using k8s;
using k8s.Models;

namespace KD.Infrastructure.k8s.ViewModels;

internal class KubernetesDataLoader : IKubernetesDataLoader
{
    private readonly IKubernetesClientManager _clientManager;

    public KubernetesDataLoader(IKubernetesClientManager clientManager)
    {
        _clientManager = clientManager;
    }

    private Kubernetes GetClient(Context context)
    {
        var client = _clientManager.GetClient(context.Name);

        if (client == null) throw new ArgumentNullException(nameof(client));

        return client;
    }

    public async Task<V1ClusterRoleBindingList> GetClusterRoleBindings(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        var client = GetClient(context);
        return await client.ListClusterRoleBindingAsync(cancellationToken: cancellationToken);
    }

    public async Task<V1ClusterRoleList> GetClusterRoles(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        var client = GetClient(context);
        return await client.ListClusterRoleAsync(cancellationToken: cancellationToken);
    }

    public async Task<V1ConfigMapList> GetConfigMaps(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        var client = GetClient(context);
        return await client.ListConfigMapForAllNamespacesAsync(cancellationToken: cancellationToken);
    }

    public async Task<V1CronJobList> GetCronJobs(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        var client = GetClient(context);
        return await client.ListCronJobForAllNamespacesAsync(cancellationToken: cancellationToken);
    }

    public async Task<V1CustomResourceDefinitionList> GetCustomResourceDefinitions(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        var client = GetClient(context);
        return await client.ListCustomResourceDefinitionAsync(cancellationToken: cancellationToken);
    }

    public async Task<V1DaemonSetList> GetDaemonSets(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        var client = GetClient(context);
        return await client.ListDaemonSetForAllNamespacesAsync(cancellationToken: cancellationToken);
    }

    public async Task<V1DeploymentList> GetDeployments(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        var client = GetClient(context);
        return await client.ListDeploymentForAllNamespacesAsync(cancellationToken: cancellationToken);
    }

    public async Task<V1EndpointsList> GetEndpoints(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        var client = GetClient(context);
        return await client.ListEndpointsForAllNamespacesAsync(cancellationToken: cancellationToken);
    }

    public async Task<Eventsv1EventList> GetEvents(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        var client = GetClient(context);
        return await client.EventsV1.ListEventForAllNamespacesAsync(cancellationToken: cancellationToken);
    }

    public async Task<V2HorizontalPodAutoscalerList> GetHorizontalPodAutoscalers(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        var client = GetClient(context);
        return await client.AutoscalingV2.ListHorizontalPodAutoscalerForAllNamespacesAsync(cancellationToken: cancellationToken);
    }

    public async Task<V1IngressClassList> GetIngressClasses(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        var client = GetClient(context);
        return await client.ListIngressClassAsync(cancellationToken: cancellationToken);
    }

    public async Task<V1IngressList> GetIngresses(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        var client = GetClient(context);
        return await client.ListIngressForAllNamespacesAsync(cancellationToken: cancellationToken);
    }

    public async Task<V1JobList> GetJobs(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        var client = GetClient(context);
        return await client.ListJobForAllNamespacesAsync(cancellationToken: cancellationToken);
    }

    public async Task<V1LeaseList> GetLeases(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        var client = GetClient(context);
        return await client.ListLeaseForAllNamespacesAsync(cancellationToken: cancellationToken);
    }

    public async Task<V1MutatingWebhookConfigurationList> GetMutatingWebhookConfigurations(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        var client = GetClient(context);
        return await client.ListMutatingWebhookConfigurationAsync(cancellationToken: cancellationToken);
    }

    public async Task<V1NamespaceList> GetNamespaces(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        var client = GetClient(context);
        return await client.ListNamespaceAsync(cancellationToken: cancellationToken);
    }

    public async Task<V1NetworkPolicyList> GetNetworkPolicies(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        var client = GetClient(context);
        return await client.ListNetworkPolicyForAllNamespacesAsync(cancellationToken: cancellationToken);
    }

    public async Task<V1PersistentVolumeClaimList> GetPersistentVolumeClaims(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        var client = GetClient(context);
        return await client.ListPersistentVolumeClaimForAllNamespacesAsync(cancellationToken: cancellationToken);
    }

    public async Task<V1PersistentVolumeList> GetPersistentVolumes(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        var client = GetClient(context);
        return await client.ListPersistentVolumeAsync(cancellationToken: cancellationToken);
    }

    public async Task<V1PodDisruptionBudgetList> GetPodDisruptionBudgets(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        var client = GetClient(context);
        return await client.ListPodDisruptionBudgetForAllNamespacesAsync(cancellationToken: cancellationToken);
    }

    public async Task<V1PodList> GetPods(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        var client = GetClient(context);
        V1PodList? pods = null;

        if (namespaces.Length > 0)
        {
            foreach (var ns in namespaces)
            {
                if (pods == null)
                {
                    pods = await client.ListNamespacedPodAsync(namespaceParameter: ns, cancellationToken: cancellationToken);
                }
                else
                {
                    var list = await client.ListNamespacedPodAsync(namespaceParameter: ns, cancellationToken: cancellationToken);
                    pods.Items = pods.Items.Concat(list.Items).ToList();
                }
            }
        }
        else
        {
            pods = await client.ListPodForAllNamespacesAsync(cancellationToken: cancellationToken);
        }

        return pods!;
    }

    public async Task<V1PriorityClassList> GetPriorityClasses(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        var client = GetClient(context);
        return await client.ListPriorityClassAsync(cancellationToken: cancellationToken);
    }

    public async Task<V1ReplicaSetList> GetReplicaSets(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        var client = GetClient(context);
        return await client.ListReplicaSetForAllNamespacesAsync(cancellationToken: cancellationToken);
    }

    public async Task<V1ReplicationControllerList> GetReplicationControllers(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        var client = GetClient(context);
        return await client.ListReplicationControllerForAllNamespacesAsync(cancellationToken: cancellationToken);
    }

    public async Task<V1ResourceQuotaList> GetResourceQuotas(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        var client = GetClient(context);
        return await client.ListResourceQuotaForAllNamespacesAsync(cancellationToken: cancellationToken);
    }

    public async Task<V1RoleBindingList> GetRoleBindings(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        var client = GetClient(context);
        return await client.ListRoleBindingForAllNamespacesAsync(cancellationToken: cancellationToken);
    }

    public async Task<V1RoleList> GetRoles(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        var client = GetClient(context);
        return await client.ListRoleForAllNamespacesAsync(cancellationToken: cancellationToken);
    }

    public async Task<V1RuntimeClassList> GetRuntimeClasses(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        var client = GetClient(context);
        return await client.ListRuntimeClassAsync(cancellationToken: cancellationToken);
    }

    public async Task<V1SecretList> GetSecrets(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        var client = GetClient(context);
        return await client.ListSecretForAllNamespacesAsync(cancellationToken: cancellationToken);
    }

    public async Task<V1ServiceAccountList> GetServiceAccounts(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        var client = GetClient(context);
        return await client.ListServiceAccountForAllNamespacesAsync(cancellationToken: cancellationToken);
    }

    public async Task<V1ServiceList> GetServices(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        var client = GetClient(context);
        return await client.ListServiceForAllNamespacesAsync(cancellationToken: cancellationToken);
    }

    public async Task<V1StatefulSetList> GetStatefulSets(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        var client = GetClient(context);
        return await client.ListStatefulSetForAllNamespacesAsync(cancellationToken: cancellationToken);
    }

    public async Task<V1StorageClassList> GetStorageClasses(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        var client = GetClient(context);
        return await client.ListStorageClassAsync(cancellationToken: cancellationToken);
    }

    public async Task<V1ValidatingWebhookConfigurationList> GetValidatingWebhookConfigurations(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        var client = GetClient(context);
        return await client.ListValidatingWebhookConfigurationAsync(cancellationToken: cancellationToken);
    }

    public async Task<V1NodeList> GetNodes(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        var client = GetClient(context);
        return await client.ListNodeAsync(cancellationToken: cancellationToken);
    }

    public async Task<V1Pod?> GetPod(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        var client = GetClient(context);
        var pod = await client.ReadNamespacedPodAsync(name, ns, false, cancellationToken);
        return pod;
    }

    public async Task<V1Deployment?> GetDeployment(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        var client = GetClient(context);
        var deployment = await client.ReadNamespacedDeploymentAsync(name, ns, false, cancellationToken);
        return deployment;
    }

    public async Task<V1Endpoints?> GetEndpoint(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        var client = GetClient(context);
        var endpoint = await client.ReadNamespacedEndpointsAsync(name, ns, false, cancellationToken);
        return endpoint;
    }

    public async Task<V1Ingress?> GetIngress(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        var client = GetClient(context);
        var ingress = await client.ReadNamespacedIngressAsync(name, ns, false, cancellationToken);
        return ingress;
    }

    public async Task<V1Namespace?> GetNamespace(Context context, string @namespace, string name, CancellationToken cancellationToken)
    {
        var client = GetClient(context);
        var ns = await client.ReadNamespaceAsync(name, false, cancellationToken);
        return ns;
    }

    public async Task<V1Service?> GetService(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        var client = GetClient(context);
        var service = await client.ReadNamespacedServiceAsync(name, ns, false, cancellationToken);
        return service;
    }

    public async Task<V1Node?> GetNode(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        var client = GetClient(context);
        var node = await client.ReadNodeAsync(name, false, cancellationToken);
        return node;
    }

    public async Task<V1ClusterRoleBinding?> GetClusterRoleBinding(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        var client = GetClient(context);
        var clusterRoleBinding = await client.ReadClusterRoleBindingAsync(name, false, cancellationToken);
        return clusterRoleBinding;
    }

    public async Task<V1ClusterRole?> GetClusterRole(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        var client = GetClient(context);
        var clusterRole = await client.ReadClusterRoleAsync(name, false, cancellationToken);
        return clusterRole;
    }

    public async Task<V1ConfigMap?> GetConfigMap(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        var client = GetClient(context);
        var configMap = await client.ReadNamespacedConfigMapAsync(name, ns, false, cancellationToken);
        return configMap;
    }

    public async Task<V1CronJob?> GetCronJob(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        var client = GetClient(context);
        var cronJob = await client.ReadNamespacedCronJobAsync(name, ns, false, cancellationToken);
        return cronJob;
    }

    public async Task<V1CustomResourceDefinition?> GetCustomResourcesDefinition(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        var client = GetClient(context);
        var customResourceDefinition = await client.ReadCustomResourceDefinitionAsync(name, false, cancellationToken);
        return customResourceDefinition;
    }

    public async Task<V1DaemonSet?> GetDaemonSet(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        var client = GetClient(context);
        var daemonSet = await client.ReadNamespacedDaemonSetAsync(name, ns, false, cancellationToken);
        return daemonSet;
    }

    public async Task<V2HorizontalPodAutoscaler?> GetHorizontalPodAutoscaler(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        var client = GetClient(context);
        var horizontalPodAutoscaler = await client.AutoscalingV2.ReadNamespacedHorizontalPodAutoscalerAsync(name, ns, false, cancellationToken);
        return horizontalPodAutoscaler;
    }

    public async Task<V1IngressClass?> GetIngressClass(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        var client = GetClient(context);
        var ingressClass = await client.ReadIngressClassAsync(name, false, cancellationToken);
        return ingressClass;
    }

    public async Task<V1Job?> GetJob(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        var client = GetClient(context);
        var job = await client.ReadNamespacedJobAsync(name, ns, false, cancellationToken);
        return job;
    }

    public async Task<V1Lease?> GetLease(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        var client = GetClient(context);
        var lease = await client.ReadNamespacedLeaseAsync(name, ns, false, cancellationToken);
        return lease;
    }

    public async Task<V1MutatingWebhookConfiguration?> GetMutatingWebhookConfiguration(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        var client = GetClient(context);
        var mutatingWebhookConfiguration = await client.ReadMutatingWebhookConfigurationAsync(name, false, cancellationToken);
        return mutatingWebhookConfiguration;
    }

    public async Task<V1NetworkPolicy?> GetNetworkPolicy(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        var client = GetClient(context);
        var networkPolicy = await client.ReadNamespacedNetworkPolicyAsync(name, ns, false, cancellationToken);
        return networkPolicy;
    }

    public async Task<V1PersistentVolume?> GetPersistentVolume(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        var client = GetClient(context);
        var pv = await client.ReadPersistentVolumeAsync(name, false, cancellationToken);
        return pv;
    }

    public async Task<V1PersistentVolumeClaim?> GetPersistentVolumeClaim(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        var client = GetClient(context);
        var pvc = await client.ReadNamespacedPersistentVolumeClaimAsync(name, ns, false, cancellationToken);
        return pvc;
    }

    public async Task<V1PodDisruptionBudget?> GetPodDisruptionBudget(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        var client = GetClient(context);
        var pdb = await client.ReadNamespacedPodDisruptionBudgetAsync(name, ns, false, cancellationToken);
        return pdb;
    }

    public async Task<V1PriorityClass?> GetPriorityClass(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        var client = GetClient(context);
        var priorityClass = await client.ReadPriorityClassAsync(name, false, cancellationToken);
        return priorityClass;
    }

    public async Task<V1ReplicaSet?> GetReplicaSet(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        var client = GetClient(context);
        var replicaSet = await client.ReadNamespacedReplicaSetAsync(name, ns, false, cancellationToken);
        return replicaSet;
    }

    public async Task<V1ReplicationController?> GetReplicationController(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        var client = GetClient(context);
        var replicationController = await client.ReadNamespacedReplicationControllerAsync(name, ns, false, cancellationToken);
        return replicationController;
    }

    public async Task<V1ResourceQuota?> GetResourceQuota(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        var client = GetClient(context);
        var resourceQuota = await client.ReadNamespacedResourceQuotaAsync(name, ns, false, cancellationToken);
        return resourceQuota;
    }

    public async Task<V1Role?> GetRole(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        var client = GetClient(context);
        var role = await client.ReadNamespacedRoleAsync(name, ns, false, cancellationToken);
        return role;
    }

    public async Task<V1RoleBinding?> GetRoleBinding(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        var client = GetClient(context);
        var roleBinding = await client.ReadNamespacedRoleBindingAsync(name, ns, false, cancellationToken);
        return roleBinding;
    }

    public async Task<V1RuntimeClass?> GetRuntimeClass(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        var client = GetClient(context);
        var runtimeClass = await client.ReadRuntimeClassAsync(name, false, cancellationToken);
        return runtimeClass;
    }

    public async Task<V1Secret?> GetSecret(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        var client = GetClient(context);
        var secret = await client.ReadNamespacedSecretAsync(name, ns, false, cancellationToken);
        return secret;
    }

    public async Task<V1ServiceAccount?> GetServiceAccount(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        var client = GetClient(context);
        var serviceAccount = await client.ReadNamespacedServiceAccountAsync(name, ns, false, cancellationToken);
        return serviceAccount;
    }

    public async Task<V1StatefulSet?> GetStatefulSet(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        var client = GetClient(context);
        var statefulSet = await client.ReadNamespacedStatefulSetAsync(name, ns, false, cancellationToken);
        return statefulSet;
    }

    public async Task<V1StorageClass?> GetStorageClass(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        var client = GetClient(context);
        var storageClass = await client.ReadStorageClassAsync(name, false, cancellationToken);
        return storageClass;
    }

    public async Task<V1ValidatingWebhookConfiguration?> GetValidatingWebhookConfiguration(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        var client = GetClient(context);
        var vwhc = await client.ReadValidatingWebhookConfigurationAsync(name, false, cancellationToken);
        return vwhc;
    }
}
