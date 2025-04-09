using Microsoft.Extensions.Logging;
using System.Threading;
using TaskManager.Application.Services;
using TaskManager.SharedKernel.Constants;

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
            _logger.LogInformation(Miscellaneous.TaskReminderLog, tarefa.Title);
        }
    }
}