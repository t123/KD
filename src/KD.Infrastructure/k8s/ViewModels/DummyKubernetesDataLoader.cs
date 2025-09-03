using k8s.Models;
using System.Text.Json;

namespace KD.Infrastructure.k8s.ViewModels;

internal class DummyKubernetesDataLoader : IKubernetesDataLoader
{
    private async Task<T> Get<T>(string name, string filename)
    {
        try
        {
            const string Dir = @"F:\git\KD\data";
            var file = Path.Combine(Dir, name, $"{filename}.json");
            using var stream = File.OpenRead(file);
            T? obj = await JsonSerializer.DeserializeAsync<T>(stream);

            if (obj == null) throw new ArgumentNullException(nameof(obj));

            return obj;
        }
        catch (AggregateException e1)
        {
            return default;
        }
        catch (Exception e)
        {
            return default;
        }
    }

    public async Task<V1ClusterRoleBindingList> GetClusterRoleBindings(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        var obj = await Get<V1ClusterRoleBindingList>(context.Name, ObjectType.ClusterRoleBinding);
        if (namespaces.Length > 0)
        {
            obj.Items = obj.Items.Where(x => namespaces.Contains(x.Namespace())).ToList();
        }

        return obj;
    }

    public async Task<V1ClusterRoleList> GetClusterRoles(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        var obj = await Get<V1ClusterRoleList>(context.Name, ObjectType.ClusterRole);
        if (namespaces.Length > 0)
        {
            obj.Items = obj.Items.Where(x => namespaces.Contains(x.Namespace())).ToList();
        }

        return obj;
    }

    public async Task<V1ConfigMapList> GetConfigMaps(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        var obj = await Get<V1ConfigMapList>(context.Name, ObjectType.ConfigMap);
        if (namespaces.Length > 0)
        {
            obj.Items = obj.Items.Where(x => namespaces.Contains(x.Namespace())).ToList();
        }

        return obj;
    }

    public async Task<V1CronJobList> GetCronJobs(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        var obj = await Get<V1CronJobList>(context.Name, ObjectType.CronJob);
        if (namespaces.Length > 0)
        {
            obj.Items = obj.Items.Where(x => namespaces.Contains(x.Namespace())).ToList();
        }

        return obj;
    }

    public async Task<V1CustomResourceDefinitionList> GetCustomResourceDefinitions(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        var obj = await Get<V1CustomResourceDefinitionList>(context.Name, ObjectType.CustomResourcesDefinition);
        if (namespaces.Length > 0)
        {
            obj.Items = obj.Items.Where(x => namespaces.Contains(x.Namespace())).ToList();
        }

        return obj;
    }

    public async Task<V1DaemonSetList> GetDaemonSets(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        var obj = await Get<V1DaemonSetList>(context.Name, ObjectType.DaemonSet);
        if (namespaces.Length > 0)
        {
            obj.Items = obj.Items.Where(x => namespaces.Contains(x.Namespace())).ToList();
        }

        return obj;
    }

    public async Task<V1DeploymentList> GetDeployments(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        var obj = await Get<V1DeploymentList>(context.Name, ObjectType.Deployment);
        if (namespaces.Length > 0)
        {
            obj.Items = obj.Items.Where(x => namespaces.Contains(x.Namespace())).ToList();
        }

        return obj;
    }

    public async Task<V1EndpointsList> GetEndpoints(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        var obj = await Get<V1EndpointsList>(context.Name, ObjectType.Endpoint);
        if (namespaces.Length > 0)
        {
            obj.Items = obj.Items.Where(x => namespaces.Contains(x.Namespace())).ToList();
        }

        return obj;
    }

    public async Task<Eventsv1EventList> GetEvents(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        var obj = await Get<Eventsv1EventList>(context.Name, ObjectType.Event);
        if (namespaces.Length > 0)
        {
            obj.Items = obj.Items.Where(x => namespaces.Contains(x.Namespace())).ToList();
        }

        return obj;
    }

    public async Task<V2HorizontalPodAutoscalerList> GetHorizontalPodAutoscalers(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        var obj = await Get<V2HorizontalPodAutoscalerList>(context.Name, ObjectType.HorizontalPodAutoscaler);
        if (namespaces.Length > 0)
        {
            obj.Items = obj.Items.Where(x => namespaces.Contains(x.Namespace())).ToList();
        }

        return obj;
    }

    public async Task<V1IngressClassList> GetIngressClasses(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        var obj = await Get<V1IngressClassList>(context.Name, ObjectType.IngressClass);
        if (namespaces.Length > 0)
        {
            obj.Items = obj.Items.Where(x => namespaces.Contains(x.Namespace())).ToList();
        }

        return obj;
    }

    public async Task<V1IngressList> GetIngresses(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        var obj = await Get<V1IngressList>(context.Name, ObjectType.Ingress);
        if (namespaces.Length > 0)
        {
            obj.Items = obj.Items.Where(x => namespaces.Contains(x.Namespace())).ToList();
        }

        return obj;
    }

    public async Task<V1JobList> GetJobs(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        var obj = await Get<V1JobList>(context.Name, ObjectType.Job);
        if (namespaces.Length > 0)
        {
            obj.Items = obj.Items.Where(x => namespaces.Contains(x.Namespace())).ToList();
        }

        return obj;
    }

    public async Task<V1LeaseList> GetLeases(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        var obj = await Get<V1LeaseList>(context.Name, ObjectType.Lease);
        if (namespaces.Length > 0)
        {
            obj.Items = obj.Items.Where(x => namespaces.Contains(x.Namespace())).ToList();
        }

        return obj;
    }

    public async Task<V1MutatingWebhookConfigurationList> GetMutatingWebhookConfigurations(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        var obj = await Get<V1MutatingWebhookConfigurationList>(context.Name, ObjectType.MutatingWebhookConfiguration);
        if (namespaces.Length > 0)
        {
            obj.Items = obj.Items.Where(x => namespaces.Contains(x.Namespace())).ToList();
        }

        return obj;
    }

    public async Task<V1NamespaceList> GetNamespaces(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        var obj = await Get<V1NamespaceList>(context.Name, ObjectType.Namespace);
        if (namespaces.Length > 0)
        {
            obj.Items = obj.Items.Where(x => namespaces.Contains(x.Name())).ToList();
        }

        return obj;
    }

    public async Task<V1NetworkPolicyList> GetNetworkPolicies(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        var obj = await Get<V1NetworkPolicyList>(context.Name, ObjectType.NetworkPolicy);
        if (namespaces.Length > 0)
        {
            obj.Items = obj.Items.Where(x => namespaces.Contains(x.Namespace())).ToList();
        }

        return obj;
    }

    public async Task<V1PersistentVolumeClaimList> GetPersistentVolumeClaims(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        var obj = await Get<V1PersistentVolumeClaimList>(context.Name, ObjectType.PersistentVolumeClaim);
        if (namespaces.Length > 0)
        {
            obj.Items = obj.Items.Where(x => namespaces.Contains(x.Namespace())).ToList();
        }

        return obj;
    }

    public async Task<V1PersistentVolumeList> GetPersistentVolumes(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        var obj = await Get<V1PersistentVolumeList>(context.Name, ObjectType.PersistentVolume);
        if (namespaces.Length > 0)
        {
            obj.Items = obj.Items.Where(x => namespaces.Contains(x.Namespace())).ToList();
        }

        return obj;
    }

    public async Task<V1PodDisruptionBudgetList> GetPodDisruptionBudgets(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        var obj = await Get<V1PodDisruptionBudgetList>(context.Name, ObjectType.PodDisruptionBudget);
        if (namespaces.Length > 0)
        {
            obj.Items = obj.Items.Where(x => namespaces.Contains(x.Namespace())).ToList();
        }

        return obj;
    }

    public async Task<V1PodList> GetPods(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        var pods = await Get<V1PodList>(context.Name, ObjectType.Pod);
        if (namespaces.Length > 0)
        {
            pods.Items = pods.Items.Where(pod => namespaces.Contains(pod.Namespace())).ToList();
        }

        return pods;
    }

    public async Task<V1PriorityClassList> GetPriorityClasses(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        var obj = await Get<V1PriorityClassList>(context.Name, ObjectType.PriorityClass);
        if (namespaces.Length > 0)
        {
            obj.Items = obj.Items.Where(x => namespaces.Contains(x.Namespace())).ToList();
        }

        return obj;
    }

    public async Task<V1ReplicaSetList> GetReplicaSets(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        var obj = await Get<V1ReplicaSetList>(context.Name, ObjectType.ReplicaSet);
        if (namespaces.Length > 0)
        {
            obj.Items = obj.Items.Where(x => namespaces.Contains(x.Namespace())).ToList();
        }

        return obj;
    }

    public async Task<V1ReplicationControllerList> GetReplicationControllers(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        var obj = await Get<V1ReplicationControllerList>(context.Name, ObjectType.ReplicationController);
        if (namespaces.Length > 0)
        {
            obj.Items = obj.Items.Where(x => namespaces.Contains(x.Namespace())).ToList();
        }

        return obj;
    }

    public async Task<V1ResourceQuotaList> GetResourceQuotas(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        var obj = await Get<V1ResourceQuotaList>(context.Name, ObjectType.ResourceQuota);
        if (namespaces.Length > 0)
        {
            obj.Items = obj.Items.Where(x => namespaces.Contains(x.Namespace())).ToList();
        }

        return obj;
    }

    public async Task<V1RoleBindingList> GetRoleBindings(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        var obj = await Get<V1RoleBindingList>(context.Name, ObjectType.RoleBinding);
        if (namespaces.Length > 0)
        {
            obj.Items = obj.Items.Where(x => namespaces.Contains(x.Namespace())).ToList();
        }

        return obj;
    }

    public async Task<V1RoleList> GetRoles(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        var obj = await Get<V1RoleList>(context.Name, ObjectType.Role);
        if (namespaces.Length > 0)
        {
            obj.Items = obj.Items.Where(x => namespaces.Contains(x.Namespace())).ToList();
        }

        return obj;
    }

    public async Task<V1RuntimeClassList> GetRuntimeClasses(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        var obj = await Get<V1RuntimeClassList>(context.Name, ObjectType.RuntimeClass);
        if (namespaces.Length > 0)
        {
            obj.Items = obj.Items.Where(x => namespaces.Contains(x.Namespace())).ToList();
        }

        return obj;
    }

    public async Task<V1SecretList> GetSecrets(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        var obj = await Get<V1SecretList>(context.Name, ObjectType.Secret);
        if (namespaces.Length > 0)
        {
            obj.Items = obj.Items.Where(x => namespaces.Contains(x.Namespace())).ToList();
        }

        return obj;
    }

    public async Task<V1ServiceAccountList> GetServiceAccounts(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        var obj = await Get<V1ServiceAccountList>(context.Name, ObjectType.ServiceAccount);
        if (namespaces.Length > 0)
        {
            obj.Items = obj.Items.Where(x => namespaces.Contains(x.Namespace())).ToList();
        }

        return obj;
    }

    public async Task<V1ServiceList> GetServices(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        var obj = await Get<V1ServiceList>(context.Name, ObjectType.Service);
        if (namespaces.Length > 0)
        {
            obj.Items = obj.Items.Where(x => namespaces.Contains(x.Namespace())).ToList();
        }

        return obj;
    }

    public async Task<V1StatefulSetList> GetStatefulSets(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        var obj = await Get<V1StatefulSetList>(context.Name, ObjectType.StatefulSet);
        if (namespaces.Length > 0)
        {
            obj.Items = obj.Items.Where(x => namespaces.Contains(x.Namespace())).ToList();
        }

        return obj;
    }

    public async Task<V1StorageClassList> GetStorageClasses(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        var obj = await Get<V1StorageClassList>(context.Name, ObjectType.StorageClass);
        if (namespaces.Length > 0)
        {
            obj.Items = obj.Items.Where(x => namespaces.Contains(x.Namespace())).ToList();
        }

        return obj;
    }

    public async Task<V1ValidatingWebhookConfigurationList> GetValidatingWebhookConfigurations(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        var obj = await Get<V1ValidatingWebhookConfigurationList>(context.Name, ObjectType.ValidatingWebhookConfiguration);
        if (namespaces.Length > 0)
        {
            obj.Items = obj.Items.Where(x => namespaces.Contains(x.Namespace())).ToList();
        }

        return obj;
    }

    public async Task<V1NodeList> GetNodes(Context context, string[] namespaces, CancellationToken cancellationToken)
    {
        var obj = await Get<V1NodeList>(context.Name, ObjectType.Node);
        if (namespaces.Length > 0)
        {
            obj.Items = obj.Items.Where(x => namespaces.Contains(x.Namespace())).ToList();
        }

        return obj;
    }

    public async Task<V1Pod?> GetPod(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        var list = await GetPods(context, [ns], cancellationToken);
        var item = list.Items.SingleOrDefault(x => x.Metadata.Name == name && x.Metadata.Namespace() == ns);
        return item;
    }

    public async Task<V1Deployment?> GetDeployment(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        var list = await GetDeployments(context, [ns], cancellationToken);
        var item = list.Items.SingleOrDefault(x => x.Metadata.Name == name && x.Metadata.Namespace() == ns);
        return item;
    }

    public async Task<V1Endpoints?> GetEndpoint(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        var list = await GetEndpoints(context, [ns], cancellationToken);
        var item = list.Items.SingleOrDefault(x => x.Metadata.Name == name && x.Metadata.Namespace() == ns);
        return item;
    }

    public async Task<V1Ingress?> GetIngress(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        var list = await GetIngresses(context, [ns], cancellationToken);
        var item = list.Items.SingleOrDefault(x => x.Metadata.Name == name && x.Metadata.Namespace() == ns);
        return item;
    }

    public async Task<V1Namespace?> GetNamespace(Context context, string @namespace, string name, CancellationToken cancellationToken)
    {
        var list = await GetNamespaces(context, [name], cancellationToken);
        var item = list.Items.SingleOrDefault(x => x.Metadata.Name == name);
        return item;
    }

    public async Task<V1Service?> GetService(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        var list = await GetServices(context, [ns], cancellationToken);
        var item = list.Items.SingleOrDefault(x => x.Metadata.Name == name && x.Metadata.Namespace() == ns);
        return item;
    }

    public async Task<V1Node?> GetNode(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        var list = await GetNodes(context, [], cancellationToken);
        var item = list.Items.SingleOrDefault(x => x.Metadata.Name == name);
        return item;
    }

    public async Task<V1ClusterRoleBinding?> GetClusterRoleBinding(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        var list = await GetClusterRoleBindings(context, [ns], cancellationToken);
        var item = list.Items.SingleOrDefault(x => x.Metadata.Name == name && x.Metadata.Namespace() == ns);
        return item;
    }

    public async Task<V1ClusterRole?> GetClusterRole(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        var list = await GetClusterRoles(context, [ns], cancellationToken);
        var item = list.Items.SingleOrDefault(x => x.Metadata.Name == name && x.Metadata.Namespace() == ns);
        return item;
    }

    public async Task<V1ConfigMap?> GetConfigMap(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        var list = await GetConfigMaps(context, [ns], cancellationToken);
        var item = list.Items.SingleOrDefault(x => x.Metadata.Name == name && x.Metadata.Namespace() == ns);
        return item;
    }

    public async Task<V1CronJob?> GetCronJob(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        var list = await GetCronJobs(context, [ns], cancellationToken);
        var item = list.Items.SingleOrDefault(x => x.Metadata.Name == name && x.Metadata.Namespace() == ns);
        return item;
    }

    public async Task<V1CustomResourceDefinition?> GetCustomResourcesDefinition(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        var list = await GetCustomResourceDefinitions(context, [ns], cancellationToken);
        var item = list.Items.SingleOrDefault(x => x.Metadata.Name == name && x.Metadata.Namespace() == ns);
        return item;
    }

    public async Task<V1DaemonSet?> GetDaemonSet(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        var list = await GetDaemonSets(context, [ns], cancellationToken);
        var item = list.Items.SingleOrDefault(x => x.Metadata.Name == name && x.Metadata.Namespace() == ns);
        return item;
    }

    public async Task<V2HorizontalPodAutoscaler?> GetHorizontalPodAutoscaler(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        var list = await GetHorizontalPodAutoscalers(context, [ns], cancellationToken);
        var item = list.Items.SingleOrDefault(x => x.Metadata.Name == name && x.Metadata.Namespace() == ns);
        return item;
    }

    public async Task<V1IngressClass?> GetIngressClass(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        var list = await GetIngressClasses(context, [ns], cancellationToken);
        var item = list.Items.SingleOrDefault(x => x.Metadata.Name == name && x.Metadata.Namespace() == ns);
        return item;
    }

    public async Task<V1Job?> GetJob(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        var list = await GetJobs(context, [ns], cancellationToken);
        var item = list.Items.SingleOrDefault(x => x.Metadata.Name == name && x.Metadata.Namespace() == ns);
        return item;
    }

    public async Task<V1Lease?> GetLease(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        var list = await GetLeases(context, [ns], cancellationToken);
        var item = list.Items.SingleOrDefault(x => x.Metadata.Name == name && x.Metadata.Namespace() == ns);
        return item;
    }

    public async Task<V1MutatingWebhookConfiguration?> GetMutatingWebhookConfiguration(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        var list = await GetMutatingWebhookConfigurations(context, [ns], cancellationToken);
        var item = list.Items.SingleOrDefault(x => x.Metadata.Name == name && x.Metadata.Namespace() == ns);
        return item;
    }

    public async Task<V1NetworkPolicy?> GetNetworkPolicy(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        var list = await GetNetworkPolicies(context, [ns], cancellationToken);
        var item = list.Items.SingleOrDefault(x => x.Metadata.Name == name && x.Metadata.Namespace() == ns);
        return item;
    }

    public async Task<V1PersistentVolume?> GetPersistentVolume(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        var list = await GetPersistentVolumes(context, [ns], cancellationToken);
        var item = list.Items.SingleOrDefault(x => x.Metadata.Name == name && x.Metadata.Namespace() == ns);
        return item;
    }

    public async Task<V1PersistentVolumeClaim?> GetPersistentVolumeClaim(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        var list = await GetPersistentVolumeClaims(context, [ns], cancellationToken);
        var item = list.Items.SingleOrDefault(x => x.Metadata.Name == name && x.Metadata.Namespace() == ns);
        return item;
    }

    public async Task<V1PodDisruptionBudget?> GetPodDisruptionBudget(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        var list = await GetPodDisruptionBudgets(context, [ns], cancellationToken);
        var item = list.Items.SingleOrDefault(x => x.Metadata.Name == name && x.Metadata.Namespace() == ns);
        return item;
    }

    public async Task<V1PriorityClass?> GetPriorityClass(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        var list = await GetPriorityClasses(context, [ns], cancellationToken);
        var item = list.Items.SingleOrDefault(x => x.Metadata.Name == name && x.Metadata.Namespace() == ns);
        return item;
    }

    public async Task<V1ReplicaSet?> GetReplicaSet(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        var list = await GetReplicaSets(context, [ns], cancellationToken);
        var item = list.Items.SingleOrDefault(x => x.Metadata.Name == name && x.Metadata.Namespace() == ns);
        return item;
    }

    public async Task<V1ReplicationController?> GetReplicationController(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        var list = await GetReplicationControllers(context, [ns], cancellationToken);
        var item = list.Items.SingleOrDefault(x => x.Metadata.Name == name && x.Metadata.Namespace() == ns);
        return item;
    }

    public async Task<V1ResourceQuota?> GetResourceQuota(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        var list = await GetResourceQuotas(context, [ns], cancellationToken);
        var item = list.Items.SingleOrDefault(x => x.Metadata.Name == name && x.Metadata.Namespace() == ns);
        return item;
    }

    public async Task<V1Role?> GetRole(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        var list = await GetRoles(context, [ns], cancellationToken);
        var item = list.Items.SingleOrDefault(x => x.Metadata.Name == name && x.Metadata.Namespace() == ns);
        return item;
    }

    public async Task<V1RoleBinding?> GetRoleBinding(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        var list = await GetRoleBindings(context, [ns], cancellationToken);
        var item = list.Items.SingleOrDefault(x => x.Metadata.Name == name && x.Metadata.Namespace() == ns);
        return item;
    }

    public async Task<V1RuntimeClass?> GetRuntimeClass(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        var list = await GetRuntimeClasses(context, [ns], cancellationToken);
        var item = list.Items.SingleOrDefault(x => x.Metadata.Name == name && x.Metadata.Namespace() == ns);
        return item;
    }

    public async Task<V1Secret?> GetSecret(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        var list = await GetSecrets(context, [ns], cancellationToken);
        var item = list.Items.SingleOrDefault(x => x.Metadata.Name == name && x.Metadata.Namespace() == ns);
        return item;
    }

    public async Task<V1ServiceAccount?> GetServiceAccount(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        var list = await GetServiceAccounts(context, [ns], cancellationToken);
        var item = list.Items.SingleOrDefault(x => x.Metadata.Name == name && x.Metadata.Namespace() == ns);
        return item;
    }

    public async Task<V1StatefulSet?> GetStatefulSet(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        var list = await GetStatefulSets(context, [ns], cancellationToken);
        var item = list.Items.SingleOrDefault(x => x.Metadata.Name == name && x.Metadata.Namespace() == ns);
        return item;
    }

    public async Task<V1StorageClass?> GetStorageClass(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        var list = await GetStorageClasses(context, [ns], cancellationToken);
        var item = list.Items.SingleOrDefault(x => x.Metadata.Name == name && x.Metadata.Namespace() == ns);
        return item;
    }

    public async Task<V1ValidatingWebhookConfiguration?> GetValidatingWebhookConfiguration(Context context, string ns, string name, CancellationToken cancellationToken)
    {
        var list = await GetValidatingWebhookConfigurations(context, [ns], cancellationToken);
        var item = list.Items.SingleOrDefault(x => x.Metadata.Name == name && x.Metadata.Namespace() == ns);
        return item;
    }
}