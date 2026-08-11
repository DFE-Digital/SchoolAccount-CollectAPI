---
status: "accepted"
date: 2026-08-10
decision-makers: Kian Winstanley
---

# Use containerisation to publish code

## Context and Problem Statement

A decision has been made to use containerisation to publish code to the github container registry. 
This ensures that the same immutable image is used and promoted to all environments to ensure consistency. 
This is to be done rather than deploying code onto services. 

## Decision Drivers

* An immutable image ensures exactly the same code is run from development through to production.
* An image can be pulled directly from the registry and run without needing to build it locally.

## Considered Options

* Deploying packaged code onto web servers
* Use containerisation to publish code

## Decision Outcome

Chosen option: "Use containerisation to publish code", because it aligns with DfE strategy for service deployment.

### Consequences

* Good, because it creates an immutable image that provides consistency across all environments.
* Good, because it allows containers to be run in pipelines for end to end tests.
* Bad, because it adds additional complexity in the build pipeline.
* Bad, because it means we have to manage image life cycles and security vulnerabilities.

### Confirmation

The image has been build and published to the [container registry](https://github.com/DFE-Digital/SchoolAccount-CollectAPI/pkgs/container/schoolaccount-collectapi).