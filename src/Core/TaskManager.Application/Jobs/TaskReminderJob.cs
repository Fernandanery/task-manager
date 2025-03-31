using Microsoft.Extensions.Logging;
using TaskManager.Application.Services;

public class TaskReminderJob
{
    private readonly TaskService _taskService;
    private readonly ILogger<TaskReminderJob> _logger;

    public TaskReminderJob(TaskService taskService, ILogger<TaskReminderJob> logger)
    {
        _taskService = taskService;
        _logger = logger;
    }

    public async Task EnviarLembretesDeTarefasPendentes()
    {
        var tarefasPendentes = await _taskService.GetPendingTasksAsync();
        foreach (var tarefa in tarefasPendentes)
        {
            _logger.LogInformation("Lembrete: Tarefa '{Titulo}' ainda está pendente.", tarefa.Title);
        }
    }
}