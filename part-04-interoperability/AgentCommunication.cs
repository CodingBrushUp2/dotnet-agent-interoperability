namespace AgentInteroperability.Part04;

public sealed record AgentRequest(string IncidentId, string Service, DateTimeOffset From, DateTimeOffset To);
public sealed record AgentEvidence(string Reference, string Observation);
public sealed record AgentResult(AgentStatus Status, IReadOnlyList<AgentEvidence> Evidence);
public enum AgentStatus { Succeeded, Unavailable }

public interface IDeploymentAgent
{
    Task<IReadOnlyList<AgentEvidence>> AnalyzeAsync(AgentRequest request, CancellationToken ct);
}

// Illustrative call boundary, not an A2A client. Authentication belongs in the adapter.
public sealed class AgentCommunication(IDeploymentAgent agent)
{
    public async Task<AgentResult> AnalyzeAsync(AgentRequest request, CancellationToken ct)
    {
        using var deadline = CancellationTokenSource.CreateLinkedTokenSource(ct);
        deadline.CancelAfter(TimeSpan.FromSeconds(10));

        try
        {
            var evidence = await agent.AnalyzeAsync(request, deadline.Token);
            ct.ThrowIfCancellationRequested();
            if (deadline.IsCancellationRequested)
                return new AgentResult(AgentStatus.Unavailable, []);
            return new AgentResult(AgentStatus.Succeeded, evidence);
        }
        catch (OperationCanceledException) when (ct.IsCancellationRequested) { throw; }
        catch (OperationCanceledException) when (deadline.IsCancellationRequested)
        {
            return new AgentResult(AgentStatus.Unavailable, []);
        }
    }
}
