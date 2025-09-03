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
    public required V1ClusterRoleBinding ClusterRoleBinding { get; init; }
}

public class ClusterRolePropertyViewModel : BasePropertyViewModel
{
    public override string ObjectPropertyType => ObjectType.ClusterRole;
    public required V1ClusterRole ClusterRole { get; init; }
}

public class ConfigMapPropertyViewModel : BasePropertyViewModel
{
    public override string ObjectPropertyType => ObjectType.ConfigMap;
    public required V1ConfigMap ConfigMap { get; init; }
}

public class CronJobPropertyViewModel : BasePropertyViewModel
{
    public override string ObjectPropertyType => ObjectType.CronJob;
    public required V1CronJob CronJob { get; init; }

}

public class CustomResourcesDefinitionPropertyViewModel : BasePropertyViewModel
{
    public override string ObjectPropertyType => ObjectType.CustomResourcesDefinition;
    public required V1CustomResourceDefinition CustomResourceDefinition { get; init; }

}

public class DaemonSetPropertyViewModel : BasePropertyViewModel
{
    public override string ObjectPropertyType => ObjectType.DaemonSet;
    public required V1DaemonSet DaemonSet { get; init; }

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
    public required V2HorizontalPodAutoscaler HorizontalPodAutoscaler { get; init; }

}

public class IngressClassPropertyViewModel : BasePropertyViewModel
{
    public override string ObjectPropertyType => ObjectType.IngressClass;
    public required V1IngressClass IngressClass { get; init; }
}

public class IngressPropertyViewModel : BasePropertyViewModel
{
    public override string ObjectPropertyType => ObjectType.Ingress;
    public required V1Ingress Ingress { get; init; }
}

public class JobPropertyViewModel : BasePropertyViewModel
{
    public override string ObjectPropertyType => ObjectType.Job;
    public required V1Job Job { get; init; }
}

public class LeasePropertyViewModel : BasePropertyViewModel
{
    public override string ObjectPropertyType => ObjectType.Lease;
    public required V1Lease Lease { get; init; }
}

public class MutatingWebhookConfigurationPropertyViewModel : BasePropertyViewModel
{
    public override string ObjectPropertyType => ObjectType.MutatingWebhookConfiguration;
    public required V1MutatingWebhookConfiguration MutatingWebhookConfiguration { get; init; }
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
    public required V1NetworkPolicy NetworkPolicy { get; init; }

}

public class PersistentVolumePropertyViewModel : BasePropertyViewModel
{
    public override string ObjectPropertyType => ObjectType.PersistentVolume;
    public required V1PersistentVolume PersistentVolume { get; init; }
}

public class PersistentVolumeClaimPropertyViewModel : BasePropertyViewModel
{
    public override string ObjectPropertyType => ObjectType.PersistentVolumeClaim;
    public required V1PersistentVolumeClaim PersistentVolumeClaim { get; init; }
}

public class PodDisruptionBudgetPropertyViewModel : BasePropertyViewModel
{
    public override string ObjectPropertyType => ObjectType.PodDisruptionBudget;
    public required V1PodDisruptionBudget PodDisruptionBudget { get; init; }

}

public class PortForwardingPropertyViewModel : BasePropertyViewModel
{
    public override string ObjectPropertyType => ObjectType.PortForwarding;
}

public class PriorityClassPropertyViewModel : BasePropertyViewModel
{
    public override string ObjectPropertyType => ObjectType.PriorityClass;
    public required V1PriorityClass PriorityClass { get; init; }
}

public class ReplicaSetPropertyViewModel : BasePropertyViewModel
{
    public override string ObjectPropertyType => ObjectType.ReplicaSet;
    public required V1ReplicaSet ReplicaSet { get; init; }
}

public class ReplicationControllerPropertyViewModel : BasePropertyViewModel
{
    public override string ObjectPropertyType => ObjectType.ReplicationController;
    public required V1ReplicationController ReplicationController { get; init; }
}

public class ResourceQuotaPropertyViewModel : BasePropertyViewModel
{
    public override string ObjectPropertyType => ObjectType.ResourceQuota;
    public required V1ResourceQuota ResourceQuota { get; init; }
}

public class RoleBindingPropertyViewModel : BasePropertyViewModel
{
    public override string ObjectPropertyType => ObjectType.RoleBinding;
    public required V1RoleBinding RoleBinding { get; init; }
}

public class RolePropertyViewModel : BasePropertyViewModel
{
    public override string ObjectPropertyType => ObjectType.Role;
    public required V1Role Role { get; init; }
}

public class RuntimeClassPropertyViewModel : BasePropertyViewModel
{
    public override string ObjectPropertyType => ObjectType.RuntimeClass;
    public required V1RuntimeClass RuntimeClass { get; init; }
}

public class SecretPropertyViewModel : BasePropertyViewModel
{
    public override string ObjectPropertyType => ObjectType.Secret;
    public required V1Secret Secret { get; init; }
}

public class ServiceAccountPropertyViewModel : BasePropertyViewModel
{
    public override string ObjectPropertyType => ObjectType.ServiceAccount;
    public required V1ServiceAccount ServiceAccount { get; init; }
}

public class ServicePropertyViewModel : BasePropertyViewModel
{
    public override string ObjectPropertyType => ObjectType.Service;
    public required V1Service Service { get; init; }
}

public class StatefulSetPropertyViewModel : BasePropertyViewModel
{
    public override string ObjectPropertyType => ObjectType.StatefulSet;
    public required V1StatefulSet StatefulSet { get; init; }
}

public class StorageClassPropertyViewModel : BasePropertyViewModel
{
    public override string ObjectPropertyType => ObjectType.StorageClass;
    public required V1StorageClass StorageClass { get; init; }
}

public class ValidatingWebhookConfigurationPropertyViewModel : BasePropertyViewModel
{
    public override string ObjectPropertyType => ObjectType.ValidatingWebhookConfiguration;
    public required V1ValidatingWebhookConfiguration ValidatingWebhookConfiguration { get; init; }
}