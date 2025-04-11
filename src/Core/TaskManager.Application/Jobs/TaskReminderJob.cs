using Microsoft.Extensions.Logging;
using TaskManager.Application.Services;
using TaskManager.SharedKernel.Constants;

public class TaskReminderJob(TaskService taskService, ILogger<TaskReminderJob> logger)
{
    private readonly TaskService _taskService = taskService;
    private readonly ILogger<TaskReminderJob> _logger = logger;

    public async Task EnviarLembretesDeTarefasPendentes()
    {
        var tarefasPendentes = await _taskService.GetPendingTasksAsync();
        foreach (var tarefa in tarefasPendentes)
        {
            _logger.LogInformation(Miscellaneous.TaskReminderLog, tarefa.Title);
        }
    }
}