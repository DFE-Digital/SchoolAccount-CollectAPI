---
status: "accepted"
date: 2026-10-16
decision-makers: Paul Custance
---

# Use the Azure Monitor OpenTelemetry distro for telemetry

## Context and Problem Statement

We want logs, traces and metrics from the API in Azure Monitor, alongside the web app's, and we already log through
Serilog. How do we get telemetry into Application Insights without replacing Serilog, and how do we see the same
telemetry locally in Rider?

SchoolAccount-Web answered the same question in its
[ADR 0012](https://github.com/DFE-Digital/SchoolAccount-Web/blob/main/decisions/0012-use-azure-monitor-opentelemetry-distro.md),
which compares the options in full. This records that the API does the same.

## Decision Drivers

* Keep Serilog as the logging API, along with what we already write to the console and Seq.
* Get metrics into Application Insights, not just logs and traces.
* No extra infrastructure to run.
* See the same signals in Rider's OpenTelemetry window while developing.
* Instrument the API the same way as the web app, so one request can be followed across both.

## Considered Options

* The Azure Monitor OpenTelemetry distro, with Serilog forwarding to it
* Plain OTLP straight to Application Insights' OTLP ingestion
* `Serilog.Sinks.OpenTelemetry` with the OpenTelemetry endpoints that the Container Apps managed agent provides

## Decision Outcome

Chosen option: "The Azure Monitor OpenTelemetry distro, with Serilog forwarding to it", for the reasons in the web
app's ADR: it sends metrics to Application Insights as well as logs and traces, it needs nothing extra to run, and it
works with the shared Application Insights resource and its connection string.

How it works:

* Telemetry goes to one destination, never both, chosen explicitly by `Telemetry:Destination` in appsettings:
  * `AzureMonitor` in `appsettings.json`, so deployed environments use the distro. It sends logs, traces and metrics
    to Application Insights using `APPLICATIONINSIGHTS_CONNECTION_STRING`, which comes from the shared Azure App
    Configuration store.
  * `Otlp` in `appsettings.Development.json`, so local runs send logs, traces and metrics over OTLP to
    `OTEL_EXPORTER_OTLP_ENDPOINT`, which Rider sets for runs from the IDE. Locally this also traces SQL queries, which
    the distro does when deployed.
  * `None` when the setting is missing, so nothing is registered.
* Settings for the destination that isn't chosen are ignored, so environment variables added by tools can't move
  telemetry somewhere unexpected.
* If the chosen destination isn't configured the API fails at startup, so a deployed API can't silently lose its
  telemetry. The exception is OTLP in development, where running without a collector, for example through Docker
  Compose or the integration tests, leaves telemetry off.
* Serilog forwards its events to the OpenTelemetry logger provider, so logs line up with their traces. The default
  logging providers are cleared so the console isn't printed twice.
* The role name and instance come from the distro's Container Apps detector (the container app name and the replica).
  Locally, the launch profile sets the service name to `schoolaccount-collectapi` and the instance id to `local`.
* Serilog's per-request summary is turned down to `Warning`, as in the web app, because Application Insights already
  records each request.

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
destination are ignored, and when startup fails because the chosen destination isn't configured.
The first deployment should show telemetry in Application Insights with the container app name as the role and the
replica as the instance.

## More Information

The comparison of the options, and when to revisit this, is in the web app's ADR. If the web app moves to OTLP
ingestion, this decision should move with it.
