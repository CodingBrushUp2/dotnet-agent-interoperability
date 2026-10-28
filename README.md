# .NET Agent Interoperability

Small architecture sketches accompanying the four-part **Building Enterprise AI Systems** series. These are intentionally concise examples, not a runnable product or a protocol implementation.

| Article | Example | Main idea |
| --- | --- | --- |
| [Part 1: What Changes When Agents Need to Work Together?](https://www.alihaghighi.pro/blog/ai-mesh-architecture-part-1) | [Concepts](part-01-concepts/README.md) | MCP, A2A, gateway, and mesh |
| [Part 2: Designing an AI Mesh](https://www.alihaghighi.pro/blog/ai-mesh-architecture-part-2) | [Decisions](part-02-architecture/README.md) | Boundaries and tradeoffs |
| [Part 3: Bounded Coordinator in .NET](https://www.alihaghighi.pro/blog/ai-mesh-architecture-part-3) | [Coordinator.cs](part-03-coordinator/Coordinator.cs) | Typed evidence and partial failure |
| [Part 4: Independent .NET Agents](https://www.alihaghighi.pro/blog/ai-mesh-architecture-part-4) | [AgentCommunication.cs](part-04-interoperability/AgentCommunication.cs) | Communication contracts and deadlines |

The snippets show control flow, not enforced request deadlines: cancellation must be honored by the downstream adapter. They have not been compiled or tested as a standalone project.

No model, MCP server, A2A endpoint, or mesh infrastructure is running here. Source files are focused teaching sketches without a project file, build pipeline, or production security configuration.
