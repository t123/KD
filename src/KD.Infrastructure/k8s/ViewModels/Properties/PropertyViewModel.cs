using k8s.Models;
using KD.Infrastructure.k8s.Fluxor;

namespace KD.Infrastructure.k8s.ViewModels.Properties;

public interface IPropertyViewModel
{
    string ObjectPropertyType { get; }
    TabModel Tab { get; }
    string Uid { get; }
    string Name { get; }
    DateTime? Created { get; }
}

public abstract class BasePropertyViewModel : IPropertyViewModel
{
    public required TabModel Tab { get; init; }
    public required string Uid { get; init; }
    public required string Name { get; init; }
    public required DateTime? Created { get; init; }
    public abstract string ObjectPropertyType { get; }
}

public class PodPropertyViewModel : BasePropertyViewModel
{
    public override string ObjectPropertyType => ObjectType.Pod;
    public required V1Pod Pod { get; init; }
}

public class ClusterRoleBindingPropertyViewModel : BasePropertyViewModel
{
    public override string ObjectPropertyType => ObjectType.ClusterRoleBinding;
}

public class ClusterRolePropertyViewModel : BasePropertyViewModel
{
    public override string ObjectPropertyType => ObjectType.ClusterRole;
}

public class ConfigMapPropertyViewModel : BasePropertyViewModel
{
    public override string ObjectPropertyType => ObjectType.ConfigMap;
}

public class CronJobPropertyViewModel : BasePropertyViewModel
{
    public override string ObjectPropertyType => ObjectType.CronJob;
}

public class CustomResourcesDefinitionPropertyViewModel : BasePropertyViewModel
{
    public override string ObjectPropertyType => ObjectType.CustomResourcesDefinition;
}

public class DaemonSetPropertyViewModel : BasePropertyViewModel
{
    public override string ObjectPropertyType => ObjectType.DaemonSet;
}

public class DeploymentPropertyViewModel : BasePropertyViewModel
{
    public override string ObjectPropertyType => ObjectType.Deployment;
    public required V1Deployment Deployment { get; init; }
}

public class EndpointPropertyViewModel : BasePropertyViewModel
{
    public override string ObjectPropertyType => ObjectType.Endpoint;
    public required V1Endpoints Endpoint { get; init; }
}

public class EventsPropertyViewModel : BasePropertyViewModel
{
    public override string ObjectPropertyType => ObjectType.Event;
}

public class HorizontalPodAutoscalerPropertyViewModel : BasePropertyViewModel
{
    public override string ObjectPropertyType => ObjectType.HorizontalPodAutoscaler;
}

public class IngressClassPropertyViewModel : BasePropertyViewModel
{
    public override string ObjectPropertyType => ObjectType.IngressClass;
}

public class IngressPropertyViewModel : BasePropertyViewModel
{
    public override string ObjectPropertyType => ObjectType.Ingress;
    public required V1Ingress Ingress { get; init; }
}

public class JobPropertyViewModel : BasePropertyViewModel
{
    public override string ObjectPropertyType => ObjectType.Job;
}

public class LeasePropertyViewModel : BasePropertyViewModel
{
    public override string ObjectPropertyType => ObjectType.Lease;
}

public class MutatingWebhookConfigurationPropertyViewModel : BasePropertyViewModel
{
    public override string ObjectPropertyType => ObjectType.MutatingWebhookConfiguration;
}

public class NamespacePropertyViewModel : BasePropertyViewModel
{
    public override string ObjectPropertyType => ObjectType.Namespace;
    public required V1Namespace Namespace { get; init; }
}

public class NodePropertyViewModel : BasePropertyViewModel
{
    public override string ObjectPropertyType => ObjectType.Node;
    public required V1Node Node { get; init; }
}

public class NetworkPolicyPropertyViewModel : BasePropertyViewModel
{
    public override string ObjectPropertyType => ObjectType.NetworkPolicy;
}

public class PersistentVolumePropertyViewModel : BasePropertyViewModel
{
    public override string ObjectPropertyType => ObjectType.PersistentVolume;
}

public class PersistentVolumeClaimPropertyViewModel : BasePropertyViewModel
{
    public override string ObjectPropertyType => ObjectType.PersistentVolumeClaim;
}

public class PodDisruptionBudgetPropertyViewModel : BasePropertyViewModel
{
    public override string ObjectPropertyType => ObjectType.PodDisruptionBudget;
}

public class PortForwardingPropertyViewModel : BasePropertyViewModel
{
    public override string ObjectPropertyType => ObjectType.PortForwarding;
}

public class PriorityClassPropertyViewModel : BasePropertyViewModel
{
    public override string ObjectPropertyType => ObjectType.PriorityClass;
}

public class ReplicaSetPropertyViewModel : BasePropertyViewModel
{
    public override string ObjectPropertyType => ObjectType.ReplicaSet;
}

public class ReplicationControllerPropertyViewModel : BasePropertyViewModel
{
    public override string ObjectPropertyType => ObjectType.ReplicationController;
}

public class ResourceQuotaPropertyViewModel : BasePropertyViewModel
{
    public override string ObjectPropertyType => ObjectType.ResourceQuota;
}

public class RoleBindingPropertyViewModel : BasePropertyViewModel
{
    public override string ObjectPropertyType => ObjectType.RoleBinding;
}

public class RolePropertyViewModel : BasePropertyViewModel
{
    public override string ObjectPropertyType => ObjectType.Role;
}

public class RuntimeClassPropertyViewModel : BasePropertyViewModel
{
    public override string ObjectPropertyType => ObjectType.RuntimeClass;
}

public class SecretPropertyViewModel : BasePropertyViewModel
{
    public override string ObjectPropertyType => ObjectType.Secret;
}

public class ServiceAccountPropertyViewModel : BasePropertyViewModel
{
    public override string ObjectPropertyType => ObjectType.ServiceAccount;
}

public class ServicePropertyViewModel : BasePropertyViewModel
{
    public override string ObjectPropertyType => ObjectType.Service;
    public required V1Service Service { get; init; }
}

public class StatefulSetPropertyViewModel : BasePropertyViewModel
{
    public override string ObjectPropertyType => ObjectType.StatefulSet;
}

public class StorageClassPropertyViewModel : BasePropertyViewModel
{
    public override string ObjectPropertyType => ObjectType.StorageClass;
}

public class ValidatingWebhookConfigurationPropertyViewModel : BasePropertyViewModel
{
    public override string ObjectPropertyType => ObjectType.ValidatingWebhookConfiguration;
}