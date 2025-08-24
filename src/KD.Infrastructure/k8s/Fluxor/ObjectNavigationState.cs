using Fluxor;

namespace KD.Infrastructure.k8s.Fluxor;

[FeatureState]
public record ObjectNavigationState
{
    public ObjectNavigationItemViewModel[] Flattened
    {
        get
        {
            return Items.SelectMany(x => x?.Items?.Select(x => x) ?? []).Concat(Items.Select(x => x)).OrderBy(x => x.Name).ToArray();
        }
    }

    public ObjectNavigationItemViewModel[] Items { get; set; } =
    [
        new ObjectNavigationItemViewModel("", "Overview", "", false),
        new ObjectNavigationItemViewModel("", "Applications", "", false),
        new ObjectNavigationItemViewModel("", "Nodes", ObjectType.Node, false),

        new ObjectNavigationItemViewModel("Workloads", "Workloads", "", false,
        [
            new ObjectNavigationItemViewModel("Workloads", "Overview", "", false),
            new ObjectNavigationItemViewModel("Workloads", "Pods", ObjectType.Pod, true),
            new ObjectNavigationItemViewModel("Workloads", "Deployments", ObjectType.Deployment, true),
            new ObjectNavigationItemViewModel("Workloads", "Daemon Sets", ObjectType.DaemonSet, false),
            new ObjectNavigationItemViewModel("Workloads", "Stateful Sets", ObjectType.StatefulSet, false),
            new ObjectNavigationItemViewModel("Workloads", "Replica Sets", ObjectType.ReplicaSet, false),
            new ObjectNavigationItemViewModel("Workloads", "Replication Controllers", ObjectType.ReplicationController, false),
            new ObjectNavigationItemViewModel("Workloads", "Jobs", ObjectType.Job, false),
            new ObjectNavigationItemViewModel("Workloads", "Cron Jobs", ObjectType.CronJob, false),
        ]),

        new ObjectNavigationItemViewModel("Config", "Config", "", false, new[]
        {
            new ObjectNavigationItemViewModel("Config", "Config Maps", ObjectType.ConfigMap, false),
            new ObjectNavigationItemViewModel("Config", "Secrets", ObjectType.Secret, false),
            new ObjectNavigationItemViewModel("Config", "Resource Quotas", ObjectType.ResourceQuota, false),
            new ObjectNavigationItemViewModel("Config", "Horizontal Pod Autoscalers", ObjectType.HorizontalPodAutoscaler, false),
            new ObjectNavigationItemViewModel("Config", "Pod Disruption Budgets", ObjectType.PodDisruptionBudget, false),
            new ObjectNavigationItemViewModel("Config", "Priority Classes", ObjectType.PriorityClass, false),
            new ObjectNavigationItemViewModel("Config", "Runtime Classes", ObjectType.RuntimeClass, false),
            new ObjectNavigationItemViewModel("Config", "Leases", ObjectType.Lease, false),
            new ObjectNavigationItemViewModel("Config", "Mutating Webhook Configurations", ObjectType.MutatingWebhookConfiguration, false),
            new ObjectNavigationItemViewModel("Config", "Validating Webhook Configurations", ObjectType.ValidatingWebhookConfiguration, false),
        }),

        new ObjectNavigationItemViewModel("Network", "Network", "", false, new[]
        {
            new ObjectNavigationItemViewModel("Network", "Services", ObjectType.Service, true),
            new ObjectNavigationItemViewModel("Network", "Endpoints", ObjectType.Endpoint, true),
            new ObjectNavigationItemViewModel("Network", "Ingresses", ObjectType.Ingress, true),
            new ObjectNavigationItemViewModel("Network", "Ingress Classes", ObjectType.IngressClass, false),
            new ObjectNavigationItemViewModel("Network", "Network Policies", ObjectType.NetworkPolicy, false),
            new ObjectNavigationItemViewModel("Network", "Port Forwarding", ObjectType.PortForwarding, false)
        }),

        new ObjectNavigationItemViewModel("Storage", "Storage", "", false, new[]
        {
            new ObjectNavigationItemViewModel("Storage", "Persistent Volume Claims", ObjectType.PersistentVolumeClaim, false),
            new ObjectNavigationItemViewModel("Storage", "Persistent Volumes", ObjectType.PersistentVolume, false),
            new ObjectNavigationItemViewModel("Storage", "Storage Classes", ObjectType.StorageClass, false)
        }),

        new ObjectNavigationItemViewModel("", "Namespaces", ObjectType.Namespace, true),
        new ObjectNavigationItemViewModel("", "Events", ObjectType.Event, false),

        new ObjectNavigationItemViewModel("Helm", "Helm", "", false, new[]
        {
            new ObjectNavigationItemViewModel("Helm", "Charts", "", false),
            new ObjectNavigationItemViewModel("Helm", "Releases", "", false),
        }),

        new ObjectNavigationItemViewModel("Access Control","Access Control", "", false, new[]
        {
            new ObjectNavigationItemViewModel("Access Control", "Service Accounts", ObjectType.ServiceAccount, false),
            new ObjectNavigationItemViewModel("Access Control", "Cluster Roles", ObjectType.ClusterRole, false),
            new ObjectNavigationItemViewModel("Access Control", "Roles", ObjectType.Role, false),
            new ObjectNavigationItemViewModel("Access Control", "Cluster Role Bindings", ObjectType.ClusterRoleBinding, false),
            new ObjectNavigationItemViewModel("Access Control", "Role Bindings", ObjectType.RoleBinding, false)
        }),

        new ObjectNavigationItemViewModel("Custom Resources", "Custom Resources", "", false, new[]
        {
            new ObjectNavigationItemViewModel("Custom Resources", "Definitions", ObjectType.CustomResourcesDefinition, false)
        }),
    ];

    public ObjectNavigationState()
    {
    }
}

public record ObjectNavigationItemViewModel(string Category, string Name, string Action, bool IsFavourite, ObjectNavigationItemViewModel[]? Items = null)
{
    public string Id => $"{Category}::{Name}";
}

public record ObjectNavigationAddFavouriteAction(string Id, CancellationToken CancellationToken = default);
public record ObjectNavigationRemoveFavouriteAction(string Id, CancellationToken CancellationToken = default);

public static partial class Reducers
{
    private static ObjectNavigationItemViewModel[] UpdateObjectNavigationList(string id, ObjectNavigationState state, bool setting)
    {
        var list = state
                .Items
                .Select(x => new ObjectNavigationItemViewModel(
                    x.Id,
                    x.Name,
                    x.Action,
                    x.Id == id ? setting : x.IsFavourite,
                    x?.Items?.Select(y => new ObjectNavigationItemViewModel
                    (
                        y.Id,
                        y.Name,
                        y.Action,
                        y.Id == id ? setting : y.IsFavourite
                    )).ToArray()
                )).ToArray();

        return list;
    }

    [ReducerMethod]
    public static ObjectNavigationState ReduceObjectNavigationAddFavouriteAction(ObjectNavigationState state, ObjectNavigationAddFavouriteAction action)
    {
        var list = UpdateObjectNavigationList(action.Id, state, true);
        return state with { Items = list };
    }

    [ReducerMethod]
    public static ObjectNavigationState ReduceObjectNavigationRemoveFavouriteAction(ObjectNavigationState state, ObjectNavigationRemoveFavouriteAction action)
    {
        var list = UpdateObjectNavigationList(action.Id, state, false);
        return state with { Items = list };
    }
}