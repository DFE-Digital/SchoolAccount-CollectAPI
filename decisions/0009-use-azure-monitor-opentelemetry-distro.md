---
status: "accepted"
date: 2026-10-24
decision-makers: Paul Custance
---

# Use the Azure Monitor OpenTelemetry distro for telemetry

## Context and Problem Statement

We want logs, traces and metrics from the API in Azure Monitor, and we already log through Serilog. The web app calls
the API on every page that shows census data, so a slow or failing page is often a slow or failing API call. How do we
get the API's telemetry into the same Application Insights resource as the web app's, so one request can be followed
from the browser to the database, without replacing Serilog? And how do we see the same telemetry locally in Rider?

SchoolAccount-Web made the same choice in its
[ADR 0012](https://github.com/DFE-Digital/SchoolAccount-Web/blob/main/decisions/0012-use-azure-monitor-opentelemetry-distro.md).

## Decision Drivers

* Follow a request across the web app and the API as one end-to-end transaction, which means both sending the same
  kind of telemetry to the same place.
* Keep Serilog as the logging API, along with what we already write to the console and Seq.
* Get metrics into Application Insights, not just logs and traces.
* No extra infrastructure to run or pay for.
* See the same signals in Rider's OpenTelemetry window while developing.

## Considered Options

* The Azure Monitor OpenTelemetry distro, with Serilog forwarding to it
* Plain OTLP straight to Application Insights' OTLP ingestion
* `Serilog.Sinks.OpenTelemetry` with the OpenTelemetry endpoints that the Container Apps managed agent provides

## Decision Outcome

Chosen option: "The Azure Monitor OpenTelemetry distro, with Serilog forwarding to it", because it's the only option
that gets all three signals into Application Insights with nothing extra to run, and because the web app already uses
it, so a request and the API calls it makes land in the same tables and join up as one transaction.

How it works:

* Telemetry goes to one destination, never both, chosen explicitly by `Telemetry:Destination` in appsettings:
  * `AzureMonitor` in `appsettings.json`, so deployed environments use the distro. It sends logs, traces and metrics
    to Application Insights using `APPLICATIONINSIGHTS_CONNECTION_STRING`, which comes from the shared Azure App
    Configuration store.
  * `Otlp` in `appsettings.Development.json`, so local runs send logs, traces and metrics over OTLP to
    `OTEL_EXPORTER_OTLP_ENDPOINT`, which Rider sets for runs from the IDE. Locally this also traces SQL queries, as the
    distro does when deployed.
  * `None` in `appsettings.IntegrationTests.json`, and when the setting is missing, so nothing is registered.
* If the chosen destination isn't configured the API fails at startup, so a deployed API can't silently lose its
  telemetry. The exception is OTLP in development, where running without a collector, for example through Docker
  Compose, leaves telemetry off.
* Serilog forwards its events to the OpenTelemetry logger provider, so logs line up with their traces. The default
  logging providers are cleared so the console isn't printed twice.
* The role name and instance come from the distro's Container Apps detector (the container app name and the replica).
  Locally, the launch profile sets the service name to `schoolaccount-collectapi` and the instance id to `local`.
* Serilog's request logging is off, as in the web app. Application Insights already records each request, with its
  status and duration, and the exceptions behind failed ones, so a summary log event per request would only repeat them.

### Choosing the destination

We also considered choosing the destination from whichever variables happen to be set: Azure Monitor when there's a
connection string, OTLP when there's an endpoint, and nothing otherwise. That needs no setting, but tools add these
variables without being asked. The Container Apps managed agent injects `OTEL_EXPORTER_OTLP_ENDPOINT` into every app in
the environment once it's switched on, and Rider plugins can add a placeholder connection string to local runs. Either
would quietly move telemetry somewhere else, and a deployed API missing its connection string would quietly send
nothing. Naming the destination in appsettings, and failing when it isn't configured, avoids both.

### Consequences

* Good, because logs, traces and metrics reach Application Insights from one in-process pipeline.
* Good, because a request from the web app and the API's handling of it share an operation id, so they show as one
  end-to-end transaction.
* Good, because the destination is explicit per environment, so it can't be changed by environment variables that
  tools add.
* Bad, because forwarding Serilog through the logger provider is fiddly. The default providers have to be cleared, and
  logging has to be turned off on the OTLP log exporter's `HttpClient`, or every log is exported twice over
  `http/protobuf`.
* Bad, because a new environment has to choose its destination, or it gets none.
* Bad, because the distro's default sampling can drop a burst of requests and the logs that go with them.

### Confirmation

Unit tests cover which destination is registered for each `Telemetry:Destination`, that settings for the other
destination are ignored, that the container app name is reported as the service name, and when startup fails because
the chosen destination isn't configured.
The first deployment should show telemetry in Application Insights with the container app name as the role and the
replica as the instance, and a web app request that calls the API should show both in one end-to-end transaction.

## Pros and Cons of the Options

### The Azure Monitor OpenTelemetry distro, with Serilog forwarding to it

`Azure.Monitor.OpenTelemetry.AspNetCore` sets up the OpenTelemetry SDK with ASP.NET Core, HttpClient and SqlClient
instrumentation and the Azure Monitor exporter in one call. Serilog writes to the registered logger providers, which
include OpenTelemetry's.

* Good, because it sends metrics as well as logs and traces.
* Good, because it needs no extra infrastructure, and works with the shared Application Insights resource and its
  connection string as they are.
* Good, because it instruments incoming requests, outgoing HTTP calls and SQL queries without any code of ours.
* Good, because it detects Container Apps itself, so the role name and instance need no configuration.
* Good, because it's what the web app uses, so the two services' telemetry lands in the same classic Application
  Insights tables and correlates.
* Good, because it can authenticate with Microsoft Entra ID if local authentication is turned off on the resource.
* Good, because it brings Live Metrics and sampling.
* Bad, because of the Serilog forwarding quirks above.
* Bad, because locally we need a second exporter (OTLP) to see telemetry in Rider, so local and deployed don't use the
  same exporter.

### Plain OTLP straight to Application Insights' OTLP ingestion

Application Insights can ingest OTLP for logs, traces and metrics when OTLP support is turned on for the resource. The
app would send OTLP to the resource's data collection endpoint for each signal, authenticated with a Microsoft Entra
token.

* Good, because it sends metrics as well as logs and traces.
* Good, because the app only needs plain OpenTelemetry packages, so local and deployed use the same exporter.
* Bad, because the shared Application Insights resource would need OTLP support turned on, and the container app's
  identity would need the Monitoring Metrics Publisher role on the resource's data collection rule.
* Bad, because the endpoints only accept a Microsoft Entra token. Microsoft documents this path for an OpenTelemetry
  Collector, so an app exporting directly needs its own token handling. The web app tried this and backed it out.
* Bad, because metrics land in an Azure Monitor workspace (a Prometheus store), and logs and traces in Log Analytics
  using the OpenTelemetry schema, rather than in the classic Application Insights tables. While the web app uses the
  distro, the two services' telemetry would be in different tables.
* Bad, because we'd lose what the distro adds, such as Live Metrics and sampling.

### `Serilog.Sinks.OpenTelemetry` with the OpenTelemetry endpoints that the Container Apps managed agent provides

Container Apps can run a managed OpenTelemetry agent for the environment. It injects the agent's OTLP endpoints into
each app (`OTEL_EXPORTER_OTLP_ENDPOINT`, plus a `CONTAINERAPP_OTEL_*_GRPC_ENDPOINT` variable for each signal). Serilog
and the OpenTelemetry SDK would send OTLP to those endpoints, and the agent would forward to Application Insights.

* Good, because the app only needs plain OTLP, so local and deployed use the same path.
* Good, because where telemetry goes is set once for the environment, not per app.
* Bad, because metrics don't reach Application Insights. The agent's Application Insights destination only accepts logs
  and traces, so metrics would need another destination such as Datadog or a separate OTLP backend.
* Bad, because it's a preview feature that runs as a single replica, can't be scaled, and drops data if the destination
  is down for more than about five minutes.
* Bad, because it can't be used if the Application Insights resource has local authentication turned off.
* Bad, because it's configured on the shared Container Apps environment, so turning it on affects every app there.

## More Information

Worth revisiting alongside the web app's decision: if the managed agent starts sending metrics to Application
Insights, or if the services move to OTLP ingestion. All the services should move together, so their telemetry stays
in one place and keeps correlating.

The agent's limitations are listed under known limitations in
[Collect and read OpenTelemetry data in Azure Container Apps](https://learn.microsoft.com/en-us/azure/container-apps/opentelemetry-agents).
