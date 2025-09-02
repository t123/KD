using k8s.Models;
using KD.Infrastructure.k8s.ViewModels.Objects;
using Microsoft.Extensions.Logging;
using static KD.Infrastructure.k8s.ViewModels.Objects.ContainerViewModel;

namespace KD.Infrastructure.k8s.ViewModels;

internal interface IViewStateHelper
{
    Task<IEnumerable<ClusterRoleBindingViewModel>?> GetClusterRoleBindings(Context context, string[] namespaces, CancellationToken cancellationToken);
    Task<IEnumerable<ClusterRoleViewModel>?> GetClusterRoles(Context context, string[] namespaces, CancellationToken cancellationToken);
    Task<IEnumerable<PodViewModel>?> GetPods(Context context, string[] namespaces, CancellationToken cancellationToken);
    Task<IEnumerable<ServiceViewModel>?> GetServices(Context context, string[] namespaces, CancellationToken cancellationToken);
    Task<IEnumerable<ConfigMapViewModel>?> GetConfigMaps(Context context, string[] namespaces, CancellationToken cancellationToken);
    Task<IEnumerable<CronJobViewModel>?> GetCronJobs(Context context, string[] namespaces, CancellationToken cancellationToken);
    Task<IEnumerable<CustomResourcesDefinitionViewModel>?> GetCustomResources(Context context, string[] namespaces, CancellationToken cancellationToken);
    Task<IEnumerable<DaemonSetViewModel>?> GetDaemonSets(Context context, string[] namespaces, CancellationToken cancellationToken);
    Task<IEnumerable<DeploymentViewModel>?> GetDeployments(Context context, string[] namespaces, CancellationToken cancellationToken);
    Task<IEnumerable<EndpointViewModel>?> GetEndpoints(Context context, string[] namespaces, CancellationToken cancellationToken);
    Task<IEnumerable<EventsViewModel>?> GetEvents(Context context, string[] namespaces, CancellationToken cancellationToken);
    Task<IEnumerable<HorizontalPodAutoscalerViewModel>?> GetHorizontalPodAutoscalers(Context context, string[] namespaces, CancellationToken cancellationToken);
    Task<IEnumerable<IngressClassViewModel>?> GetIngressClasses(Context context, string[] namespaces, CancellationToken cancellationToken);
    Task<IEnumerable<IngressViewModel>?> GetIngresses(Context context, string[] namespaces, CancellationToken cancellationToken);
    Task<IEnumerable<JobViewModel>?> GetJobs(Context context, string[] namespaces, CancellationToken cancellationToken);
    Task<IEnumerable<LeaseViewModel>?> GetLeases(Context context, string[] namespaces, CancellationToken cancellationToken);
    Task<IEnumerable<MutatingWebhookConfigurationViewModel>?> GetMutatingWebhookConfigurations(Context context, string[] namespaces, CancellationToken cancellationToken);
    Task<IEnumerable<NamespaceViewModel>?> GetNamespaces(Context context, string[] namespaces, CancellationToken cancellationToken);
    Task<IEnumerable<NetworkPolicyViewModel>?> GetNetworkPolicies(Context context, string[] namespaces, CancellationToken cancellationToken);
    Task<IEnumerable<PersistentVolumeClaimViewModel>?> GetPersistentVolumeClaims(Context context, string[] namespaces, CancellationToken cancellationToken);
    Task<IEnumerable<PersistentVolumeViewModel>?> GetPersistentVolumes(Context context, string[] namespaces, CancellationToken cancellationToken);
    Task<IEnumerable<PodDisruptionBudgetViewModel>?> GetPodDisruptionBudgets(Context context, string[] namespaces, CancellationToken cancellationToken);
    Task<IEnumerable<PriorityClassViewModel>?> GetPriorityClasses(Context context, string[] namespaces, CancellationToken cancellationToken);
    Task<IEnumerable<ReplicaSetViewModel>?> GetReplicaSets(Context context, string[] namespaces, CancellationToken cancellationToken);
    Task<IEnumerable<ReplicationControllerViewModel>?> GetReplicationControllers(Context context, string[] namespaces, CancellationToken cancellationToken);
    Task<IEnumerable<ResourceQuotaViewModel>?> GetResourceQuotas(Context context, string[] namespaces, CancellationToken cancellationToken);
    Task<IEnumerable<RoleBindingViewModel>?> GetRoleBindings(Context context, string[] namespaces, CancellationToken cancellationToken);
    Task<IEnumerable<RoleViewModel>?> GetRoles(Context context, string[] namespaces, CancellationToken cancellationToken);
    Task<IEnumerable<RuntimeClassViewModel>?> GetRuntimes(Context context, string[] namespaces, CancellationToken cancellationToken);
    Task<IEnumerable<SecretViewModel>?> GetSecrets(Context context, string[] namespaces, CancellationToken cancellationToken);
    Task<IEnumerable<ServiceAccountViewModel>?> GetServiceAccounts(Context context, string[] namespaces, CancellationToken cancellationToken);
    Task<IEnumerable<StatefulSetViewModel>?> GetStatefulSets(Context context, string[] namespaces, CancellationToken cancellationToken);
    Task<IEnumerable<StorageClassViewModel>?> GetStorageClasses(Context context, string[] namespaces, CancellationToken cancellationToken);
    Task<IEnumerable<ValidatingWebhookConfigurationViewModel>?> GetValidatingWebhookConfigurations(Context context, string[] namespaces, CancellationToken cancellationToken);
    Task<IEnumerable<NodeViewModel>?> GetNodes(Context context, string[] namespaces, CancellationToken cancellationToken);

    Task<V1Pod?> GetPod(Context context, string ns, string name, CancellationToken cancellationToken);
    Task<V1Deployment?> GetDeployment(Context context, string ns, string name, CancellationToken cancellationToken);
    Task<V1Endpoints?> GetEndpoint(Context context, string ns, string name, CancellationToken cancellationToken);
    Task<V1Ingress?> GetIngress(Context context, string ns, string name, CancellationToken cancellationToken);
    Task<V1Namespace?> GetNamespace(Context context, string ns, string name, CancellationToken cancellationToken);
    Task<V1Service?> GetService(Context context, string ns, string name, CancellationToken cancellationToken);
    Task<V1Node?> GetNode(Context context, string ns, string name, CancellationToken cancellationToken);
    Task<V1ClusterRoleBinding?> GetClusterRoleBinding(Context context, string ns, string name, CancellationToken cancellationToken);
    Task<V1ClusterRole?> GetClusterRole(Context context, string ns, string name, CancellationToken cancellationToken);
    Task<V1ConfigMap?> GetConfigMap(Context context, string ns, string name, CancellationToken cancellationToken);
    Task<V1CronJob?> GetCronJob(Context context, string ns, string name, CancellationToken cancellationToken);
    Task<V1CustomResourceDefinition?> GetCustomResourcesDefinition(Context context, string ns, string name, CancellationToken cancellationToken);
    Task<V1DaemonSet?> GetDaemonSet(Context context, string ns, string name, CancellationToken cancellationToken);
    Task<V2HorizontalPodAutoscaler?> GetHorizontalPodAutoscaler(Context context, string ns, string name, CancellationToken cancellationToken);
    Task<V1IngressClass?> GetIngressClass(Context context, string ns, string name, CancellationToken cancellationToken);
    Task<V1Job?> GetJob(Context context, string ns, string name, CancellationToken cancellationToken);
    Task<V1Lease?> GetLease(Context context, string ns, string name, CancellationToken cancellationToken);
    Task<V1MutatingWebhookConfiguration?> GetMutatingWebhookConfiguration(Context context, string ns, string name, CancellationToken cancellationToken);
    Task<V1NetworkPolicy?> GetNetworkPolicy(Context context, string ns, string name, CancellationToken cancellationToken);
    Task<V1PersistentVolumeClaim?> GetPersistentVolumeClaim(Context context, string ns, string name, CancellationToken cancellationToken);
    Task<V1PersistentVolume?> GetPersistentVolume(Context context, string ns, string name, CancellationToken cancellationToken);
    Task<V1PodDisruptionBudget?> GetPodDisruptionBudget(Context context, string ns, string name, CancellationToken cancellationToken);
    Task<V1PriorityClass?> GetPriorityClass(Context context, string ns, string name, CancellationToken cancellationToken);
    Task<V1ReplicaSet?> GetReplicaSet(Context context, string ns, string name, CancellationToken cancellationToken);
    Task<V1ReplicationController?> GetReplicationController(Context context, string ns, string name, CancellationToken cancellationToken);
    Task<V1ResourceQuota?> GetResourceQuota(Context context, string ns, string name, CancellationToken cancellationToken);
    Task<V1RoleBinding?> GetRoleBinding(Context context, string ns, string name, CancellationToken cancellationToken);
    Task<V1Role?> GetRole(Context context, string ns, string name, CancellationToken cancellationToken);
    Task<V1RuntimeClass?> GetRuntimeClass(Context context, string ns, string name, CancellationToken cancellationToken);
    Task<V1Secret?> GetSecret(Context context, string ns, string name, CancellationToken cancellationToken);
    Task<V1ServiceAccount?> GetServiceAccount(Context context, string ns, string name, CancellationToken cancellationToken);
    Task<V1StatefulSet?> GetStatefulSet(Context context, string ns, string name, CancellationToken cancellationToken);
    Task<V1StorageClass?> GetStorageClass(Context context, string ns, string name, CancellationToken cancellationToken);
    Task<V1ValidatingWebhookConfiguration?> GetValidatingWebhookConfiguration(Context context, string ns, string name, CancellationToken cancellationToken);
}

internal class ViewStateHelper : IViewStateHelper
{
    private readonly IKubernetesDataLoader _dataLoader;
    private readonly ILogger<ViewStateHelper> _logger;

    public ViewStateHelper(IKubernetesDataLoader dataLoader, ILogger<ViewStateHelper> logger)
    {
        _dataLoader = dataLoader;
        _logger = logger;
    }

    private async Task<T?> LogAndExecute<T>(string method, Func<Task<T?>> func)
    {
        try
        {
            _logger.LogInformation("Executing {Method}", method);
            var result = await func.Invoke();
            _logger.LogInformation("Executed {Method}", method);
            return result;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Execution failed {Method}", method);
            throw;
        }
    }

    public async Task<IEnumerable<ClusterRoleViewModel>?> GetClusterRoles(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        var func = async () =>
        {
            IEnumerable<ClusterRoleViewModel>? items = null;

            try
            {
                items = (await _dataLoader.GetClusterRoles(context, namespaces, cancellationToken))
                    .Items
                    .Select(x => new ClusterRoleViewModel(x.Uid(), x.Name(), x.Namespace(), x.CreationTimestamp()))
                    .OrderBy(x => x.Namespace)
                    .ThenBy(x => x.Name);
            }
            catch
            {
                items = null;
            }

            return items;
        };

        return await LogAndExecute("GetClusterRoles", func);
    }

    public async Task<IEnumerable<ClusterRoleBindingViewModel>?> GetClusterRoleBindings(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        IEnumerable<ClusterRoleBindingViewModel>? items = null;

        try
        {
            items = (await _dataLoader.GetClusterRoleBindings(context, namespaces, cancellationToken))
                .Items
                .Select(x => new ClusterRoleBindingViewModel(x.Uid(), x.Name(), x.Namespace(), x.CreationTimestamp()))
                .OrderBy(x => x.Namespace)
                .ThenBy(x => x.Name);
        }
        catch
        {
            items = null;
        }

        return items;
    }

    public async Task<IEnumerable<ConfigMapViewModel>?> GetConfigMaps(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        IEnumerable<ConfigMapViewModel>? items = null;

        try
        {
            items = (await _dataLoader.GetConfigMaps(context, namespaces, cancellationToken))
                .Items
                .Select(x => new ConfigMapViewModel(
                    x.Uid(),
                    x.Name(),
                    x.Namespace(),
                    x.CreationTimestamp(),
                    x.Data?.Select(y => y.Key).ToArray() ?? []
                ))
                .OrderBy(x => x.Namespace)
                .ThenBy(x => x.Name);
        }
        catch
        {
            items = null;
        }

        return items;
    }

    public async Task<IEnumerable<CronJobViewModel>?> GetCronJobs(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        IEnumerable<CronJobViewModel>? items = null;

        try
        {
            items = (await _dataLoader.GetCronJobs(context, namespaces, cancellationToken))
                .Items
                .Select(x => new CronJobViewModel(
                    x.Uid(),
                    x.Name(),
                    x.Namespace(),
                    x.CreationTimestamp(),
                    x.Spec.TimeZone,
                    x.Spec.Schedule,
                    x.Spec.Suspend,
                    x.Status.Active?.Any() ?? false,
                    x.Status.LastSuccessfulTime
                ))
                .OrderBy(x => x.Namespace)
                .ThenBy(x => x.Name);
        }
        catch
        {
            items = null;
        }

        return items;
    }

    public async Task<IEnumerable<PodViewModel>?> GetPods(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        IEnumerable<PodViewModel>? items = null;

        try
        {
            items = (await _dataLoader.GetPods(context, namespaces, cancellationToken))
                .Items
                .Select(x =>
                {
                    var readyAt = x.Status.Conditions.FirstOrDefault(x => x.Type == "Ready" && x.Status == "True")?.LastTransitionTime;
                    var pod = new PodViewModel(
                        x.Metadata.Uid,
                        x.Metadata.Name,
                        x.Metadata.NamespaceProperty,
                        x.Status.ContainerStatuses?.Select(y =>
                        {
                            string state = string.Empty;
                            if (y.State.Terminated != null) state = "Terminated";
                            else if (y.State.Waiting != null) state = "Waiting";
                            else if (y.State.Running != null) state = "Running";

                            var c = new ContainerViewModel(y.ContainerID, y.Name, y.Started ?? false, state, y.RestartCount, null);

                            return c;
                        }).ToArray() ?? [],
                        x.Status.InitContainerStatuses?.Select(y =>
                        {
                            string state = string.Empty;
                            if (y.State.Terminated != null) state = "Terminated";
                            else if (y.State.Waiting != null) state = "Waiting";
                            else if (y.State.Running != null) state = "Running";

                            int? exitCode = y.State?.Terminated?.ExitCode;

                            var c = new ContainerViewModel(y.ContainerID, y.Name, y.Started ?? false, state, y.RestartCount, exitCode);

                            return c;
                        }).ToArray() ?? [],
                        x.Status.Phase,
                        x.CreationTimestamp(),
                        readyAt,
                        x.Spec.NodeName
                    );

                    return pod;
                }
                )
                .OrderBy(x => x.Namespace)
                .ThenBy(x => x.Name);
        }
        catch
        {
            items = null;
        }

        return items;
    }

    public async Task<IEnumerable<ServiceViewModel>?> GetServices(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        IEnumerable<ServiceViewModel>? items = null;

        try
        {
            items = (await _dataLoader.GetServices(context, namespaces, cancellationToken))
                .Items
                .Select(x =>
                {
                    var internalEndpoints = new List<string>();
                    foreach (var endpoint in x.Spec.Ports)
                    {
                        string ns = (x.Namespace() ?? "") == "default" ? "" : $".{x.Namespace}";
                        string name = $"{x.Name}{ns}";

                        internalEndpoints.Add($"{endpoint.Name}:{endpoint.Port} {endpoint.Protocol}");
                        internalEndpoints.Add($"{endpoint.Name}:{endpoint.NodePort} {endpoint.Protocol}");
                    }

                    var model = new ServiceViewModel(
                        x.Uid(),
                        x.Name(),
                        x.Namespace(),
                        x.Metadata.CreationTimestamp,
                        x.Spec.Type,
                        x.Spec.ClusterIP,
                        internalEndpoints.ToArray(),
                        []
                    );
                    return model;
                })
                .OrderBy(x => x.Namespace)
                .ThenBy(x => x.Name);
        }
        catch
        {
            items = null;
        }

        return items;
    }

    public async Task<IEnumerable<CustomResourcesDefinitionViewModel>?> GetCustomResources(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        IEnumerable<CustomResourcesDefinitionViewModel>? items = null;

        try
        {
            items = (await _dataLoader.GetCustomResourceDefinitions(context, namespaces, cancellationToken))
                .Items
                .Select(x => new CustomResourcesDefinitionViewModel(
                    x.Uid(),
                    x.Name(),
                    x.Namespace(),
                    x.CreationTimestamp(),
                    x.Spec.Group,
                    x.Spec.Versions.Select(y => y.Name).ToArray(),
                    x.Spec.Scope
                ))
                .OrderBy(x => x.Namespace)
                .ThenBy(x => x.Name);
        }
        catch
        {
            items = null;
        }

        return items;
    }

    public async Task<IEnumerable<DaemonSetViewModel>?> GetDaemonSets(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        var func = async () =>
        {
            IEnumerable<DaemonSetViewModel>? items = null;

            try
            {
                items = (await _dataLoader.GetDaemonSets(context, namespaces, cancellationToken))
                    .Items
                    .Select(x => new DaemonSetViewModel(
                        x.Uid(),
                        x.Name(),
                        x.Namespace(),
                        x.CreationTimestamp(),
                        x.Status.DesiredNumberScheduled,
                        x.Status.CurrentNumberScheduled,
                        x.Status.NumberReady,
                        x.Status.UpdatedNumberScheduled,
                        x.Status.NumberAvailable
                    ))
                    .OrderBy(x => x.Namespace)
                    .ThenBy(x => x.Name);
            }
            catch
            {
                items = null;
            }

            return items;
        };

        return await LogAndExecute("GetDaemonSets", func);
    }

    public async Task<IEnumerable<DeploymentViewModel>?> GetDeployments(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        IEnumerable<DeploymentViewModel>? items = null;

        try
        {
            items = (await _dataLoader.GetDeployments(context, namespaces, cancellationToken))
                .Items
                .Select(x => new DeploymentViewModel(
                    x.Uid(),
                    x.Name(),
                    x.Namespace(),
                    x.Status.Replicas,
                    x.Status.ReadyReplicas,
                    x.Status.AvailableReplicas,
                    x.Status.UnavailableReplicas,
                    x.Status.UpdatedReplicas,
                    x.Status.TerminatingReplicas,
                    x.Metadata.CreationTimestamp))
                .OrderBy(x => x.Namespace)
                .ThenBy(x => x.Name);
        }
        catch
        {
            items = null;
        }

        return items;
    }

    public async Task<IEnumerable<EndpointViewModel>?> GetEndpoints(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        IEnumerable<EndpointViewModel>? items = null;

        try
        {
            items = (await _dataLoader.GetEndpoints(context, namespaces, cancellationToken))
                .Items
                .Select(x =>
                {
                    var ports = x.Subsets?.SelectMany(x => x.Ports?.Select(p => new PortViewModel(p.Port, p.Protocol, p.Name)) ?? []) ?? [];
                    var ready = x.Subsets?.SelectMany(x => x.Addresses?.Select(a => new AddressViewModel(a.Ip, a.Hostname)) ?? []) ?? [];
                    var notready = x.Subsets?.SelectMany(x => x.NotReadyAddresses?.Select(a => new AddressViewModel(a.Ip, a.Hostname)) ?? []) ?? [];

                    return new EndpointViewModel(x.Uid(), x.Name(), x.Namespace(), x.CreationTimestamp(), ports.ToArray(), ready.ToArray(), notready.ToArray());
                })
                .OrderBy(x => x.Namespace)
                .ThenBy(x => x.Name);
        }
        catch
        {
            items = null;
        }

        return items;
    }

    public async Task<IEnumerable<EventsViewModel>?> GetEvents(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        IEnumerable<EventsViewModel>? items = null;

        try
        {
            items = (await _dataLoader.GetEvents(context, namespaces, cancellationToken))
                .Items
                .Select(x => new EventsViewModel(
                    x.Uid(),
                    x.Name(),
                    x.Namespace(),
                    x.CreationTimestamp(),
                    x.Type,
                    x.ReportingController,
                    new ResourceViewModel(x.Regarding.Uid, x.Regarding.Kind, x.Regarding.Name),
                    x.Reason,
                    x.DeprecatedCount,
                    x.DeprecatedLastTimestamp,
                    x.DeprecatedSource.Component
                ))
                .OrderBy(x => x.Namespace)
                .ThenBy(x => x.Name);
        }
        catch
        {
            items = null;
        }

        return items;
    }

    public async Task<IEnumerable<HorizontalPodAutoscalerViewModel>?> GetHorizontalPodAutoscalers(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        IEnumerable<HorizontalPodAutoscalerViewModel>? items = null;

        try
        {
            items = (await _dataLoader.GetHorizontalPodAutoscalers(context, namespaces, cancellationToken))
                .Items
                .Select(x => new HorizontalPodAutoscalerViewModel(
                    x.Uid(),
                    x.Name(),
                    x.Namespace(),
                    x.CreationTimestamp(),
                    x.Spec.MinReplicas,
                    x.Spec.MaxReplicas,
                    x.Status.CurrentReplicas,
                    x.Status.DesiredReplicas
                ))
                .OrderBy(x => x.Namespace)
                .ThenBy(x => x.Name);
        }
        catch
        {
            items = null;
        }

        return items;
    }

    public async Task<IEnumerable<IngressClassViewModel>?> GetIngressClasses(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        IEnumerable<IngressClassViewModel>? items = null;

        try
        {
            items = (await _dataLoader.GetIngressClasses(context, namespaces, cancellationToken))
                .Items
                .Select(x => new IngressClassViewModel(x.Uid(), x.Name(), x.Namespace(), x.CreationTimestamp()))
                .OrderBy(x => x.Namespace)
                .ThenBy(x => x.Name);
        }
        catch
        {
            items = null;
        }

        return items;
    }

    public async Task<IEnumerable<IngressViewModel>?> GetIngresses(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        IEnumerable<IngressViewModel>? items = null;

        try
        {
            items = (await _dataLoader.GetIngresses(context, namespaces, cancellationToken))
                .Items.Select(x =>
                {
                    List<string> rules = new List<string>();

                    foreach (var rule in x.Spec.Rules)
                    {
                        string host = rule.Host;
                        string[] paths = rule.Http.Paths.Select(path => host + path.Path).ToArray();
                        rules.AddRange(paths);
                    }

                    var model = new IngressViewModel(
                        x.Uid(),
                        x.Name(),
                        x.Namespace(),
                        x.Metadata.CreationTimestamp,
                        rules.ToArray(),
                        x.Status?.LoadBalancer?.Ingress?.Select(x => x.Ip).ToArray() ?? []
                    );
                    return model;
                })
                .OrderBy(x => x.Namespace)
                .ThenBy(x => x.Name);
        }
        catch
        {
            items = null;
        }

        return items;
    }

    public async Task<IEnumerable<JobViewModel>?> GetJobs(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        IEnumerable<JobViewModel>? items = null;

        try
        {
            items = (await _dataLoader.GetJobs(context, namespaces, cancellationToken))
                .Items
                .Select(x => new JobViewModel(
                    x.Uid(),
                    x.Name(),
                    x.Namespace(),
                    x.CreationTimestamp(),
                    x.Status.Succeeded,
                    x.Status.Conditions?.OrderBy(y => y.LastTransitionTime).Select(y => y.Reason).FirstOrDefault() ?? "",
                    x.Status.Conditions?.OrderBy(y => y.LastTransitionTime).Select(y => y.Type).FirstOrDefault() ?? ""
                ))
                .OrderBy(x => x.Namespace)
                .ThenBy(x => x.Name);
        }
        catch
        {
            items = null;
        }

        return items;
    }

    public async Task<IEnumerable<LeaseViewModel>?> GetLeases(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        IEnumerable<LeaseViewModel>? items = null;

        try
        {
            items = (await _dataLoader.GetLeases(context, namespaces, cancellationToken))
                .Items
                .Select(x => new LeaseViewModel(x.Uid(), x.Name(), x.Namespace(), x.CreationTimestamp()))
                .OrderBy(x => x.Namespace)
                .ThenBy(x => x.Name);
        }
        catch
        {
            items = null;
        }

        return items;
    }

    public async Task<IEnumerable<MutatingWebhookConfigurationViewModel>?> GetMutatingWebhookConfigurations(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        IEnumerable<MutatingWebhookConfigurationViewModel>? items = null;

        try
        {
            items = (await _dataLoader.GetMutatingWebhookConfigurations(context, namespaces, cancellationToken))
                .Items
                .Select(x => new MutatingWebhookConfigurationViewModel(x.Uid(), x.Name(), x.Namespace(), x.CreationTimestamp()))
                .OrderBy(x => x.Namespace)
                .ThenBy(x => x.Name);
        }
        catch
        {
            items = null;
        }

        return items;
    }

    public async Task<IEnumerable<NamespaceViewModel>?> GetNamespaces(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        IEnumerable<NamespaceViewModel>? items = null;

        try
        {
            items = (await _dataLoader.GetNamespaces(context, namespaces, cancellationToken))
                .Items
                .Select(x => new NamespaceViewModel(x.Metadata.Uid, x.Metadata.Name, "", x.Metadata.CreationTimestamp, x.Status.Phase))
                .OrderBy(x => x.Name);
        }
        catch
        {
            items = null;
        }

        return items;
    }

    public async Task<IEnumerable<NetworkPolicyViewModel>?> GetNetworkPolicies(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        IEnumerable<NetworkPolicyViewModel>? items = null;

        try
        {
            items = (await _dataLoader.GetNetworkPolicies(context, namespaces, cancellationToken))
                .Items
                .Select(x => new NetworkPolicyViewModel(
                    x.Uid(),
                    x.Name(),
                    x.Namespace(),
                    x.CreationTimestamp(),
                    x.Spec.PolicyTypes.ToArray()
                ))
                .OrderBy(x => x.Namespace)
                .ThenBy(x => x.Name);
        }
        catch
        {
            items = null;
        }

        return items;
    }

    public async Task<IEnumerable<PersistentVolumeClaimViewModel>?> GetPersistentVolumeClaims(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        IEnumerable<PersistentVolumeClaimViewModel>? items = null;

        try
        {
            items = (await _dataLoader.GetPersistentVolumeClaims(context, namespaces, cancellationToken))
                .Items
                .Select(x => new PersistentVolumeClaimViewModel(
                    x.Uid(),
                    x.Name(),
                    x.Namespace(),
                    x.CreationTimestamp(),
                    x.Spec.StorageClassName
                ))
                .OrderBy(x => x.Namespace)
                .ThenBy(x => x.Name);
        }
        catch
        {
            items = null;
        }

        return items;
    }

    public async Task<IEnumerable<PersistentVolumeViewModel>?> GetPersistentVolumes(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        IEnumerable<PersistentVolumeViewModel>? items = null;

        try
        {
            items = (await _dataLoader.GetPersistentVolumes(context, namespaces, cancellationToken))
                .Items
                .Select(x => new PersistentVolumeViewModel(
                    x.Uid(),
                    x.Name(),
                    x.Namespace(),
                    x.CreationTimestamp(),
                    x.Spec.StorageClassName
                ))
                .OrderBy(x => x.Namespace)
                .ThenBy(x => x.Name);
        }
        catch
        {
            items = null;
        }

        return items;
    }

    public async Task<IEnumerable<PodDisruptionBudgetViewModel>?> GetPodDisruptionBudgets(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        IEnumerable<PodDisruptionBudgetViewModel>? items = null;

        try
        {
            items = (await _dataLoader.GetPodDisruptionBudgets(context, namespaces, cancellationToken))
                .Items
                .Select(x => new PodDisruptionBudgetViewModel(
                    x.Uid(),
                    x.Name(),
                    x.Namespace(),
                    x.CreationTimestamp(),
                    x.Spec.MinAvailable.Value,
                    x.Spec.MaxUnavailable.Value,
                    x.Status.CurrentHealthy,
                    x.Status.DesiredHealthy
                ))
                .OrderBy(x => x.Namespace)
                .ThenBy(x => x.Name);
        }
        catch
        {
            items = null;
        }

        return items;
    }

    public async Task<IEnumerable<PriorityClassViewModel>?> GetPriorityClasses(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        IEnumerable<PriorityClassViewModel>? items = null;

        try
        {
            items = (await _dataLoader.GetPriorityClasses(context, namespaces, cancellationToken))
                .Items
                .Select(x => new PriorityClassViewModel(x.Uid(), x.Name(), x.Namespace(), x.CreationTimestamp()))
                .OrderBy(x => x.Namespace)
                .ThenBy(x => x.Name);
        }
        catch
        {
            items = null;
        }

        return items;
    }

    public async Task<IEnumerable<ReplicaSetViewModel>?> GetReplicaSets(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        IEnumerable<ReplicaSetViewModel>? items = null;

        try
        {
            items = (await _dataLoader.GetReplicaSets(context, namespaces, cancellationToken))
                .Items
                .Select(x => new ReplicaSetViewModel(
                    x.Uid(),
                    x.Name(),
                    x.Namespace(),
                    x.CreationTimestamp(),
                    x.Status.ReadyReplicas,
                    x.Status.Replicas,
                    x.Status.TerminatingReplicas,
                    x.Status.AvailableReplicas
                ))
                .OrderBy(x => x.Namespace)
                .ThenBy(x => x.Name);
        }
        catch
        {
            items = null;
        }

        return items;
    }

    public async Task<IEnumerable<ReplicationControllerViewModel>?> GetReplicationControllers(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        IEnumerable<ReplicationControllerViewModel>? items = null;

        try
        {
            items = (await _dataLoader.GetReplicationControllers(context, namespaces, cancellationToken))
                .Items
                .Select(x => new ReplicationControllerViewModel(
                    x.Uid(),
                    x.Name(),
                    x.Namespace(),
                    x.CreationTimestamp(),
                    x.Status.Replicas,
                    x.Status.AvailableReplicas,
                    x.Status.ReadyReplicas
                ))
                .OrderBy(x => x.Namespace)
                .ThenBy(x => x.Name);
        }
        catch
        {
            items = null;
        }

        return items;
    }

    public async Task<IEnumerable<ResourceQuotaViewModel>?> GetResourceQuotas(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        IEnumerable<ResourceQuotaViewModel>? items = null;

        try
        {
            items = (await _dataLoader.GetResourceQuotas(context, namespaces, cancellationToken))
                .Items
                .Select(x => new ResourceQuotaViewModel(x.Uid(), x.Name(), x.Namespace(), x.CreationTimestamp()))
                .OrderBy(x => x.Namespace)
                .ThenBy(x => x.Name);
        }
        catch
        {
            items = null;
        }

        return items;
    }

    public async Task<IEnumerable<RoleBindingViewModel>?> GetRoleBindings(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        IEnumerable<RoleBindingViewModel>? items = null;

        try
        {
            items = (await _dataLoader.GetRoleBindings(context, namespaces, cancellationToken))
                .Items
                .Select(x => new RoleBindingViewModel(x.Uid(), x.Name(), x.Namespace(), x.CreationTimestamp()))
                .OrderBy(x => x.Namespace)
                .ThenBy(x => x.Name);
        }
        catch
        {
            items = null;
        }

        return items;
    }

    public async Task<IEnumerable<RoleViewModel>?> GetRoles(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        IEnumerable<RoleViewModel>? items = null;

        try
        {
            items = (await _dataLoader.GetRoles(context, namespaces, cancellationToken))
                .Items
                .Select(x => new RoleViewModel(x.Uid(), x.Name(), x.Namespace(), x.CreationTimestamp()))
                .OrderBy(x => x.Namespace)
                .ThenBy(x => x.Name);
        }
        catch
        {
            items = null;
        }

        return items;
    }

    public async Task<IEnumerable<RuntimeClassViewModel>?> GetRuntimes(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        IEnumerable<RuntimeClassViewModel>? items = null;

        try
        {
            items = (await _dataLoader.GetRuntimeClasses(context, namespaces, cancellationToken))
                .Items
                .Select(x => new RuntimeClassViewModel(x.Uid(), x.Name(), x.Namespace(), x.CreationTimestamp()))
                .OrderBy(x => x.Namespace)
                .ThenBy(x => x.Name);
        }
        catch
        {
            items = null;
        }

        return items;
    }

    public async Task<IEnumerable<SecretViewModel>?> GetSecrets(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        IEnumerable<SecretViewModel>? items = null;

        try
        {
            items = (await _dataLoader.GetSecrets(context, namespaces, cancellationToken))
                .Items
                .Select(x => new SecretViewModel(
                    x.Uid(),
                    x.Name(),
                    x.Namespace(),
                    x.CreationTimestamp(),
                    x.Data.Keys.ToArray()
                ))
                .OrderBy(x => x.Namespace)
                .ThenBy(x => x.Name);
        }
        catch
        {
            items = null;
        }

        return items;
    }

    public async Task<IEnumerable<ServiceAccountViewModel>?> GetServiceAccounts(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        IEnumerable<ServiceAccountViewModel>? items = null;

        try
        {
            items = (await _dataLoader.GetServiceAccounts(context, namespaces, cancellationToken))
                .Items
                .Select(x => new ServiceAccountViewModel(x.Uid(), x.Name(), x.Namespace(), x.CreationTimestamp()))
                .OrderBy(x => x.Namespace)
                .ThenBy(x => x.Name);
        }
        catch
        {
            items = null;
        }

        return items;
    }

    public async Task<IEnumerable<StatefulSetViewModel>?> GetStatefulSets(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        IEnumerable<StatefulSetViewModel>? items = null;

        try
        {
            items = (await _dataLoader.GetStatefulSets(context, namespaces, cancellationToken))
                .Items
                .Select(x => new StatefulSetViewModel(x.Uid(), x.Name(), x.Namespace(), x.CreationTimestamp()))
                .OrderBy(x => x.Namespace)
                .ThenBy(x => x.Name);
        }
        catch
        {
            items = null;
        }

        return items;
    }

    public async Task<IEnumerable<StorageClassViewModel>?> GetStorageClasses(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        IEnumerable<StorageClassViewModel>? items = null;

        try
        {
            items = (await _dataLoader.GetStorageClasses(context, namespaces, cancellationToken))
                .Items
                .Select(x => new StorageClassViewModel(x.Uid(), x.Name(), x.Namespace(), x.CreationTimestamp()))
                .OrderBy(x => x.Namespace)
                .ThenBy(x => x.Name);
        }
        catch
        {
            items = null;
        }

        return items;
    }

    public async Task<IEnumerable<ValidatingWebhookConfigurationViewModel>?> GetValidatingWebhookConfigurations(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        IEnumerable<ValidatingWebhookConfigurationViewModel>? items = null;

        try
        {
            items = (await _dataLoader.GetValidatingWebhookConfigurations(context, namespaces, cancellationToken))
                .Items
                .Select(x => new ValidatingWebhookConfigurationViewModel(x.Uid(), x.Name(), x.Namespace(), x.CreationTimestamp()))
                .OrderBy(x => x.Namespace)
                .ThenBy(x => x.Name);
        }
        catch
        {
            items = null;
        }

        return items;
    }

    public async Task<IEnumerable<NodeViewModel>?> GetNodes(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        IEnumerable<NodeViewModel>? items = null;

        try
        {
            items = (await _dataLoader.GetNodes(context, namespaces, cancellationToken))
                .Items
                .Select(x => new NodeViewModel(x.Uid(), x.Name(), x.Namespace(), x.CreationTimestamp()))
                .OrderBy(x => x.Namespace)
                .ThenBy(x => x.Name);
        }
        catch
        {
            items = null;
        }

        return items;
    }

    public async Task<V1Pod?> GetPod(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        return await _dataLoader.GetPod(context, ns, name, cancellationToken);
    }

    public async Task<V1Deployment?> GetDeployment(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        return await _dataLoader.GetDeployment(context, ns, name, cancellationToken);
    }

    public async Task<V1Endpoints?> GetEndpoint(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        return await _dataLoader.GetEndpoint(context, ns, name, cancellationToken);
    }

    public async Task<V1Ingress?> GetIngress(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        return await _dataLoader.GetIngress(context, ns, name, cancellationToken);
    }

    public async Task<V1Namespace?> GetNamespace(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        return await _dataLoader.GetNamespace(context, ns, name, cancellationToken);
    }

    public async Task<V1Service?> GetService(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        return await _dataLoader.GetService(context, ns, name, cancellationToken);
    }

    public async Task<V1Node?> GetNode(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        return await _dataLoader.GetNode(context, ns, name, cancellationToken);
    }

    public async Task<V1ClusterRoleBinding?> GetClusterRoleBinding(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        return await _dataLoader.GetClusterRoleBinding(context, ns, name, cancellationToken);
    }

    public async Task<V1ClusterRole?> GetClusterRole(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        var func = async () => await _dataLoader.GetClusterRole(context, ns, name, cancellationToken);

        return await LogAndExecute("GetClusterRole", func);
    }

    public async Task<V1ConfigMap?> GetConfigMap(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        return await _dataLoader.GetConfigMap(context, ns, name, cancellationToken);
    }

    public async Task<V1CronJob?> GetCronJob(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        return await _dataLoader.GetCronJob(context, ns, name, cancellationToken);
    }

    public async Task<V1CustomResourceDefinition?> GetCustomResourcesDefinition(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        return await _dataLoader.GetCustomResourcesDefinition(context, ns, name, cancellationToken);
    }

    public async Task<V1DaemonSet?> GetDaemonSet(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        return await _dataLoader.GetDaemonSet(context, ns, name, cancellationToken);
    }

    public async Task<V2HorizontalPodAutoscaler?> GetHorizontalPodAutoscaler(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        return await _dataLoader.GetHorizontalPodAutoscaler(context, ns, name, cancellationToken);
    }

    public async Task<V1IngressClass?> GetIngressClass(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        return await _dataLoader.GetIngressClass(context, ns, name, cancellationToken);
    }

    public async Task<V1Job?> GetJob(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        return await _dataLoader.GetJob(context, ns, name, cancellationToken);
    }

    public async Task<V1Lease?> GetLease(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        return await _dataLoader.GetLease(context, ns, name, cancellationToken);
    }

    public async Task<V1MutatingWebhookConfiguration?> GetMutatingWebhookConfiguration(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        return await _dataLoader.GetMutatingWebhookConfiguration(context, ns, name, cancellationToken);
    }

    public async Task<V1NetworkPolicy?> GetNetworkPolicy(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        return await _dataLoader.GetNetworkPolicy(context, ns, name, cancellationToken);
    }

    public async Task<V1PersistentVolumeClaim?> GetPersistentVolumeClaim(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        return await _dataLoader.GetPersistentVolumeClaim(context, ns, name, cancellationToken);
    }

    public async Task<V1PersistentVolume?> GetPersistentVolume(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        return await _dataLoader.GetPersistentVolume(context, ns, name, cancellationToken);
    }

    public async Task<V1PodDisruptionBudget?> GetPodDisruptionBudget(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        return await _dataLoader.GetPodDisruptionBudget(context, ns, name, cancellationToken);
    }

    public async Task<V1PriorityClass?> GetPriorityClass(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        return await _dataLoader.GetPriorityClass(context, ns, name, cancellationToken);
    }

    public async Task<V1ReplicaSet?> GetReplicaSet(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        return await _dataLoader.GetReplicaSet(context, ns, name, cancellationToken);
    }

    public async Task<V1ReplicationController?> GetReplicationController(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        return await _dataLoader.GetReplicationController(context, ns, name, cancellationToken);
    }

    public async Task<V1ResourceQuota?> GetResourceQuota(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        return await _dataLoader.GetResourceQuota(context, ns, name, cancellationToken);
    }

    public async Task<V1RoleBinding?> GetRoleBinding(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        return await _dataLoader.GetRoleBinding(context, ns, name, cancellationToken);
    }

    public async Task<V1Role?> GetRole(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        return await _dataLoader.GetRole(context, ns, name, cancellationToken);
    }

    public async Task<V1RuntimeClass?> GetRuntimeClass(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        return await _dataLoader.GetRuntimeClass(context, ns, name, cancellationToken);
    }

    public async Task<V1Secret?> GetSecret(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        return await _dataLoader.GetSecret(context, ns, name, cancellationToken);
    }

    public async Task<V1ServiceAccount?> GetServiceAccount(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        return await _dataLoader.GetServiceAccount(context, ns, name, cancellationToken);
    }

    public async Task<V1StatefulSet?> GetStatefulSet(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        return await _dataLoader.GetStatefulSet(context, ns, name, cancellationToken);
    }

    public async Task<V1StorageClass?> GetStorageClass(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        return await _dataLoader.GetStorageClass(context, ns, name, cancellationToken);
    }

    public async Task<V1ValidatingWebhookConfiguration?> GetValidatingWebhookConfiguration(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        return await _dataLoader.GetValidatingWebhookConfiguration(context, ns, name, cancellationToken);
    }
}