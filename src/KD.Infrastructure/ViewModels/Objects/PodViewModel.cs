using static KD.Infrastructure.ViewModels.Objects.ContainerViewModel;

namespace KD.Infrastructure.ViewModels.Objects;

public interface IObjectViewModel
{
    string Uid { get; }
    string Name { get; }
    string Namespace { get; }
    DateTime? Created { get; }
}

public record PodViewModel(string Uid, string Name, string Namespace, ContainerViewModel[] Containers, ContainerViewModel[] InitContainers, string Phase, DateTime? Created, DateTime? ReadyAt, string NodeName) : IObjectViewModel;
public record ContainerViewModel(string ContainerId, string Name, bool Started, ContainerState State, int RestartCount)
{
    public enum ContainerState
    {
        Unknown,
        Running,
        Waiting,
        Terminated
    }
}

public record NamespaceViewModel(string Uid, string Name, string Namespace, DateTime? Created) : IObjectViewModel;
public record IngressViewModel(string Uid, string Name, string Namespace, DateTime? Created, string[] Rules, string[] LoadBalancers) : IObjectViewModel;
public record ContextViewModel(string Name);
public record DeploymentViewModel(string Uid, string Name, string Namespace, int? Replicas, int? ReadyReplicas, int? AvailableReplicas, DateTime? Created) : IObjectViewModel;
public record ServiceViewModel(string Uid, string Name, string Namespace, DateTime? Created, string Type, string ClusterIp, string[] InternalEndpoints, string[] ExternalEndpoints) : IObjectViewModel;
public record EndpointViewModel(string Uid, string Name, string Namespace, DateTime? Created) : IObjectViewModel;
public record DaemonSetViewModel(string Uid, string Name, string Namespace, DateTime? Created, int Desired, int Current, int Ready, int? UpToDate, int? Available) : IObjectViewModel;
public record JobViewModel(string Uid, string Name, string Namespace, DateTime? Created, int? Succeeded, string Reason, string Type) : IObjectViewModel;
public record CronJobViewModel(string Uid, string Name, string Namespace, DateTime? Created, string TimeZone, string Schedule, bool? Suspend, bool Active, DateTime? LastSuccessfulTime) : IObjectViewModel;
public record StatefulSetViewModel(string Uid, string Name, string Namespace, DateTime? Created) : IObjectViewModel;
public record ReplicaSetViewModel(string Uid, string Name, string Namespace, DateTime? Created, int? ReadyReplicas, int Replicas, int? TerminatingReplicas, int? AvailableReplicas) : IObjectViewModel;
public record ReplicationControllerViewModel(string Uid, string Name, string Namespace, DateTime? Created, int Replicas, int? AvailableReplicas, int? ReadyReplicas) : IObjectViewModel;
public record ConfigMapViewModel(string Uid, string Name, string Namespace, DateTime? Created, string[] Keys) : IObjectViewModel;
public record SecretViewModel(string Uid, string Name, string Namespace, DateTime? Created, string[] Keys) : IObjectViewModel;
public record ResourceQuotaViewModel(string Uid, string Name, string Namespace, DateTime? Created) : IObjectViewModel;
public record HorizontalPodAutoscalerViewModel(string Uid, string Name, string Namespace, DateTime? Created, int? MinReplicas, int MaxReplicas, int CurrentReplicas, int DesiredReplicas, int? CurrentCPUUtilizationPercentage) : IObjectViewModel;
public record PodDisruptionBudgetViewModel(string Uid, string Name, string Namespace, DateTime? Created, string MinAvailable, string MaxUnavailable, int CurrentHealthy, int DesiredHealthy) : IObjectViewModel;
public record PriorityClassViewModel(string Uid, string Name, string Namespace, DateTime? Created) : IObjectViewModel;
public record RuntimeClassViewModel(string Uid, string Name, string Namespace, DateTime? Created) : IObjectViewModel;
public record LeaseViewModel(string Uid, string Name, string Namespace, DateTime? Created) : IObjectViewModel;
public record MutatingWebhookConfigurationViewModel(string Uid, string Name, string Namespace, DateTime? Created) : IObjectViewModel;
public record ValidatingWebhookConfigurationViewModel(string Uid, string Name, string Namespace, DateTime? Created) : IObjectViewModel;
public record IngressClassViewModel(string Uid, string Name, string Namespace, DateTime? Created) : IObjectViewModel;
public record NetworkPolicyViewModel(string Uid, string Name, string Namespace, DateTime? Created, string[] PolicyTypes) : IObjectViewModel;
public record PortForwardingViewModel(string Uid, string Name, string Namespace, DateTime? Created) : IObjectViewModel;
public record PersistentVolumeClaimViewModel(string Uid, string Name, string Namespace, DateTime? Created, string StorageClassName) : IObjectViewModel;
public record PersistentVolumeViewModel(string Uid, string Name, string Namespace, DateTime? Created, string StorageClassName) : IObjectViewModel;
public record StorageClassViewModel(string Uid, string Name, string Namespace, DateTime? Created) : IObjectViewModel;
public record EventsViewModel(string Uid, string Name, string Namespace, DateTime? Created, string Type, string ReportingController, ResourceViewModel Regarding, string Reason, int? Count, DateTime? LastSeen, string Source) : IObjectViewModel;
public record ServiceAccountViewModel(string Uid, string Name, string Namespace, DateTime? Created) : IObjectViewModel;
public record ClusterRoleViewModel(string Uid, string Name, string Namespace, DateTime? Created) : IObjectViewModel;
public record RoleViewModel(string Uid, string Name, string Namespace, DateTime? Created) : IObjectViewModel;
public record ClusterRoleBindingViewModel(string Uid, string Name, string Namespace, DateTime? Created) : IObjectViewModel;
public record RoleBindingViewModel(string Uid, string Name, string Namespace, DateTime? Created) : IObjectViewModel;
public record CustomResourcesDefinitionViewModel(string Uid, string Name, string Namespace, DateTime? Created, string Group, string[] Versions, string Scope) : IObjectViewModel;

public record ResourceViewModel(string Uid, string Kind, string Name);