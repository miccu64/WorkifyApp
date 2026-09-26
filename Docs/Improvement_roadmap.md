# WorkifyApp — Engineering Roadmap

The goal of this roadmap is to move WorkifyApp from a functional microservices project toward a production-aware system that demonstrates solid backend, frontend and distributed-systems engineering practices.

The focus is on solving real engineering problems rather than adding technologies for the sake of complexity.

---

## 1. Message Reliability

### Retry and failure handling

Add retry policies to MassTransit consumers for transient failures such as temporary database or network problems.

Retries should use a sensible backoff strategy and should not retry permanent business/validation errors indefinitely.

After retries are exhausted, failed messages should be routed to an error/dead-letter queue where they can be inspected and, where appropriate, reprocessed.

**Priority: Critical**

### Idempotent consumers

Consumers should tolerate duplicate message delivery.

Introduce a persistent mechanism for tracking processed message/event IDs and make the database enforce uniqueness. Processing the same event twice must not result in duplicated statistics or other inconsistent state.

This is one of the most important distributed-systems improvements in the project.

**Priority: Critical**

---

## 2. Integration Testing

The current unit tests should be complemented by integration tests covering the actual asynchronous workflow.

At minimum, cover:

- Workout creation → event publication → statistics update
- Consumer failure → retry → successful processing
- Duplicate event → processed only once
- Permanent consumer failure → error/dead-letter queue

Use real RabbitMQ/PostgreSQL infrastructure for these tests where practical, preferably through containers.

The goal is to prove that the system works across service and infrastructure boundaries, not only that individual classes work in isolation.

**Priority: Critical**

---

## 3. Observability

### Correlation IDs

Introduce a correlation ID for each incoming request and propagate it through service calls and published events.

A single operation should be traceable across:

`API → service → RabbitMQ → consumer → database`

The correlation ID should be present in structured logs throughout the flow.

**Priority: High**

### Structured logging

Expand the existing Serilog setup so that important operations produce structured logs containing useful identifiers such as:

- CorrelationId
- RequestId
- EventId
- UserId
- EntityId
- Service
- Duration
- Exception information

Avoid logs that only contain human-readable sentences without useful context.

**Priority: High**

### Distributed tracing

Once correlation IDs and structured logging are in place, consider OpenTelemetry for distributed tracing.

This is useful, but it should come after the basic observability foundation rather than being added immediately.

**Priority: Medium**

---

## 4. Eventual Consistency

Make eventual consistency an explicit part of the architecture.

For example:

`Workout Service → WorkoutCompleted → RabbitMQ → Statistics Service`

means that workout data and statistics may temporarily disagree.

Document where this happens, what users can observe, and how retries/idempotency help the system recover.

This should become one of the project's main distributed-systems case studies.

**Priority: High**

---

## 5. Architecture Documentation

Improve the README with a concise architecture section.

Document:

- Service responsibilities
- Database ownership
- Synchronous vs asynchronous communication
- RabbitMQ event flow
- API Gateway responsibilities
- Authentication flow
- Important architectural trade-offs

Also document why microservices were chosen for this project.

A good explanation is that the architecture is intentionally used to explore service boundaries, asynchronous communication and distributed-system problems. The domain itself does not necessarily require microservices at its current scale.

Also acknowledge that a modular monolith could be a simpler starting point for a similar product and that services should normally be extracted when there is a concrete reason to do so.

**Priority: High**

---

## 6. API Gateway

Clarify the responsibility of `Workify.Api.Gateway`.

The README should explicitly answer:

- Is it only routing traffic?
- Does it handle authentication?
- Does it perform aggregation?
- Does it handle CORS?
- Why does the frontend use the gateway instead of calling services directly?

Keep business logic out of the gateway.

If its current role is mostly routing, document that clearly rather than making the gateway appear more important than it is.

**Priority: High**

---

## 7. Message Contracts

Treat RabbitMQ events as contracts between services.

Introduce explicit event versions where appropriate, for example:

`WorkoutCreated.v1`

Define how breaking changes are handled and how consumers can be migrated without requiring every service to change simultaneously.

The goal is to show that asynchronous messages are APIs too.

**Priority: Medium**

---

## 8. Resilience

Review service-to-service communication and add sensible timeouts.

Where a dependency can repeatedly fail and cause cascading problems, consider circuit breakers.

Do not add resilience patterns just because they are common interview topics. Each one should solve an identifiable failure scenario in the application.

**Priority: Medium**

---

## 9. Frontend Quality

The frontend should demonstrate the same engineering maturity as the backend.

Focus on:

- Consistent loading states
- Consistent error handling
- Empty states
- Centralized HTTP error handling
- Feature-oriented structure
- Proper authentication handling
- Accessibility basics
- E2E coverage for the most important user flows

Avoid introducing NgRx or other state-management infrastructure unless the application actually has a state-management problem that justifies it.

**Priority: Medium**

---

## 10. Security Review

Review the application from the perspective of a real multi-user system.

Verify that:

- Users cannot access another user's resources
- Authorization is enforced server-side
- JWT validation is correct
- Secrets are not stored in source control
- Sensitive information is not written to logs
- API endpoints have appropriate validation

Security should be treated as part of the architecture rather than as a final feature.

**Priority: High**

---

## 11. CI/CD

The repository should have an automated pipeline that at least:

1. Builds the backend
2. Runs unit tests
3. Runs integration tests
4. Builds the Angular application
5. Runs frontend tests/linting
6. Builds Docker images

Later improvements can include dependency/security scanning and automated deployment.

**Priority: Medium**

---

## 12. One Deeper Technical Problem

After the reliability work is complete, implement one additional problem deeply rather than adding several superficial features.

Good candidates:

### Option A — Optimistic concurrency

Handle two clients updating the same entity and prevent lost updates.

### Option B — Load testing

Measure API throughput, response times, RabbitMQ queue growth, consumer throughput and database behavior under load.

### Option C — More complex distributed workflow

Implement a real multi-service workflow if the domain provides a good reason for one.

Pick one and document the problem, the solution, the trade-offs and what was learned.

**Priority: Medium**

---

# Recommended Implementation Order

## Phase 1 — Reliability

- [ ] MassTransit retry policies
- [ ] Error/dead-letter handling
- [ ] Idempotent consumers
- [ ] Message/event IDs

## Phase 2 — Testing

- [ ] End-to-end event integration tests
- [ ] Retry scenario
- [ ] Duplicate message scenario
- [ ] Failed message / DLQ scenario

## Phase 3 — Observability

- [ ] Correlation IDs
- [ ] Structured Serilog logging
- [ ] Distributed tracing / OpenTelemetry

## Phase 4 — Architecture

- [ ] Architecture documentation
- [ ] Eventual consistency documentation
- [ ] API Gateway responsibilities
- [ ] Microservice trade-offs
- [ ] Message versioning

## Phase 5 — Production Readiness

- [ ] Security review
- [ ] Timeouts/resilience
- [ ] Frontend error/loading states
- [ ] CI/CD pipeline
- [ ] E2E tests

## Phase 6 — One Deep Problem

- [ ] Optimistic concurrency OR
- [ ] Load testing OR
- [ ] More complex distributed workflow
