namespace AgentInteroperability.Part03;

public sealed record Evidence(string Source, string Reference, string Observation);
public sealed record ToolOutcome(string Tool, ToolStatus Status, IReadOnlyList<Evidence> Evidence);
public enum ToolStatus { Succeeded, TimedOut, Failed }

public interface IIncidentTool
{
    string Name { get; }
    Task<IReadOnlyList<Evidence>> CollectAsync(string incidentId, CancellationToken ct);
}

// Illustration: tools are trusted and few; no registration, telemetry or retry setup.
public sealed class Coordinator(IEnumerable<IIncidentTool> tools)
{
    public async Task<IReadOnlyList<ToolOutcome>> CollectAsync(string incidentId, CancellationToken ct)
    {
        return await Task.WhenAll(tools.Select(async tool =>
        {
            using var deadline = CancellationTokenSource.CreateLinkedTokenSource(ct);
            deadline.CancelAfter(TimeSpan.FromSeconds(5));

            try
            {
                var items = await tool.CollectAsync(incidentId, deadline.Token);
                ct.ThrowIfCancellationRequested();
                if (deadline.IsCancellationRequested)
                    return new ToolOutcome(tool.Name, ToolStatus.TimedOut, []);
                return new ToolOutcome(tool.Name, ToolStatus.Succeeded, items);
            }
            catch (OperationCanceledException) when (ct.IsCancellationRequested) { throw; }
            catch (OperationCanceledException) when (deadline.IsCancellationRequested)
            {
                return new ToolOutcome(tool.Name, ToolStatus.TimedOut, []);
            }
            catch (Exception)
            {
                return new ToolOutcome(tool.Name, ToolStatus.Failed, []);
            }
        }));
    }
}
