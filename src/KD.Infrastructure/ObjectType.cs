namespace KD.Infrastructure;

public static class ObjectType
{
    public const string Empty = "empty";
    public const string Pod = "pod";
    public const string Namespace = "namespace";
    public const string Ingress = "ingress";
    public const string Deployment = "deployment";
    public const string Service = "service";
    public const string Endpoint = "endpoint";
    public const string Job = "jobs";
    public const string DaemonSet = "daemonset";
    public const string CronJob = "cronjob";
    public const string StatefulSet = "statefulset";
    public const string ReplicaSet = "replicaset";
    public const string ReplicationController = "replicationcontroller";
    public const string ConfigMap = "configmap";
    public const string Secret = "secret";
    public const string ResourceQuota = "resourcequota";
    public const string HorizontalPodAutoscaler = "horizontalpodautoscaler";
    public const string PodDisruptionBudget = "poddisruptionbudget";
    public const string PriorityClass = "priorityclass";
    public const string RuntimeClass = "runtimeclass";
    public const string Lease = "lease";
    public const string MutatingWebhookConfiguration = "mutatingwebhookconfiguration";
    public const string ValidatingWebhookConfiguration = "validatingwebhookconfiguration";
    public const string IngressClass = "ingressclass";
    public const string NetworkPolicy = "networkpolicy";
    public const string PortForwarding = "portforwarding";
    public const string PersistentVolumeClaim = "persistentvolumeclaim";
    public const string PersistentVolume = "persistentvolume";
    public const string StorageClass= "storageclass";
    public const string Event = "event";
    public const string ServiceAccount = "serviceaccount";
    public const string ClusterRole = "clusterrole";
    public const string Role = "role";
    public const string ClusterRoleBinding = "clusterrolebinding";
    public const string RoleBinding = "rolebinding";
    public const string CustomResourcesDefinition = "customresourcesdefinition";
}