using Fluxor;

namespace KD.Infrastructure.Fluxor;

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

    public ObjectNavigationItemViewModel[] Items { get; set; } = new ObjectNavigationItemViewModel[]
    {
        new ObjectNavigationItemViewModel("Overview", "Overview", "", false),
        new ObjectNavigationItemViewModel("Applications", "Applications", "", false),
        new ObjectNavigationItemViewModel("Nodes", "Nodes", "", false),

        new ObjectNavigationItemViewModel("Workloads", "Workloads", "", false, new[]
        {
            new ObjectNavigationItemViewModel("Workloads_Overview", "Overview", "", false),
            new ObjectNavigationItemViewModel("Pods", "Pods", ObjectType.Pod, true),
            new ObjectNavigationItemViewModel("Deployments", "Deployments", ObjectType.Deployment, true),
            new ObjectNavigationItemViewModel("Daemon Sets", "Daemon Sets", ObjectType.DaemonSet, false),
            new ObjectNavigationItemViewModel("Stateful Sets", "Stateful Sets", ObjectType.StatefulSet, false),
            new ObjectNavigationItemViewModel("Replica Sets", "Replica Sets", ObjectType.ReplicaSet, false),
            new ObjectNavigationItemViewModel("Replication Controllers", "Replication Controllers", ObjectType.ReplicationController, false),
            new ObjectNavigationItemViewModel("Jobs", "Jobs", ObjectType.Job, false),
            new ObjectNavigationItemViewModel("Cron Jobs", "Cron Jobs", ObjectType.CronJob, false),
        }),

        new ObjectNavigationItemViewModel("Config", "Config", "", false, new[]
        {
            new ObjectNavigationItemViewModel("Config Maps", "Config Maps", ObjectType.ConfigMap, false),
            new ObjectNavigationItemViewModel("Secrets", "Secrets", ObjectType.Secret, false),
            new ObjectNavigationItemViewModel("Resource Quotas", "Resource Quotas", ObjectType.ResourceQuota, false),
            new ObjectNavigationItemViewModel("Horizontal Pod Autoscalers", "Horizontal Pod Autoscalers", ObjectType.HorizontalPodAutoscaler, false),
            new ObjectNavigationItemViewModel("Pod Disruption Budgets", "Pod Disruption Budgets", ObjectType.PodDisruptionBudget, false),
            new ObjectNavigationItemViewModel("Priority Classes", "Priority Classes", ObjectType.PriorityClass, false),
            new ObjectNavigationItemViewModel("Runtime Classes", "Runtime Classes", ObjectType.RuntimeClass, false),
            new ObjectNavigationItemViewModel("Leases", "Leases", ObjectType.Lease, false),
            new ObjectNavigationItemViewModel("Mutating Webhook Configurations", "Mutating Webhook Configurations", ObjectType.MutatingWebhookConfiguration, false),
            new ObjectNavigationItemViewModel("Validating Webhook Configurations", "Validating Webhook Configurations", ObjectType.ValidatingWebhookConfiguration, false),
        }),

        new ObjectNavigationItemViewModel("Network", "Network", "", false, new[]
        {
            new ObjectNavigationItemViewModel("Services", "Services", ObjectType.Service, true),
            new ObjectNavigationItemViewModel("Endpoints", "Endpoints", ObjectType.Endpoint, true),
            new ObjectNavigationItemViewModel("Ingresses", "Ingresses", ObjectType.Ingress, true),
            new ObjectNavigationItemViewModel("Ingress Classes", "Ingress Classes", ObjectType.IngressClass, false),
            new ObjectNavigationItemViewModel("Network Policies", "Network Policies", ObjectType.NetworkPolicy, false),
            new ObjectNavigationItemViewModel("Port Forwarding", ObjectType.PortForwarding, "", false)
        }),

        new ObjectNavigationItemViewModel("Storage", "Storage", "", false, new[]
        {
            new ObjectNavigationItemViewModel("Persistent Volume Claims", "Persistent Volume Claims", ObjectType.PersistentVolumeClaim, false),
            new ObjectNavigationItemViewModel("Persistent Volumes", "Persistent Volumes", ObjectType.PersistentVolume, false),
            new ObjectNavigationItemViewModel("Storage Classes", "Storage Classes", ObjectType.StorageClass, false)
        }),

        new ObjectNavigationItemViewModel("Namespaces", "Namespaces", ObjectType.Namespace, true),
        new ObjectNavigationItemViewModel("Events", "Events", ObjectType.Event, false),

        new ObjectNavigationItemViewModel("Helm", "Helm", "", false, new[]
        {
            new ObjectNavigationItemViewModel("Charts", "Charts", "", false),
            new ObjectNavigationItemViewModel("Releases", "Releases", "", false),
        }),

        new ObjectNavigationItemViewModel("Access Control","Access Control", "", false, new[]
        {
            new ObjectNavigationItemViewModel("Service Accounts", "Service Accounts", ObjectType.ServiceAccount, false),
            new ObjectNavigationItemViewModel("Cluster Roles", "Cluster Roles", ObjectType.ClusterRole, false),
            new ObjectNavigationItemViewModel("Roles", "Roles", ObjectType.Role, false),
            new ObjectNavigationItemViewModel("Cluster Role Bindings", "Cluster Role Bindings", ObjectType.ClusterRoleBinding, false),
            new ObjectNavigationItemViewModel("Role Bindings", "Role Bindings", ObjectType.RoleBinding, false)
        }),

        new ObjectNavigationItemViewModel("Custom Resources", "Custom Resources", "", false, new[]
        {
            new ObjectNavigationItemViewModel("Definitions", "Definitions", ObjectType.CustomResourcesDefinition, false)
        }),
    };

    public ObjectNavigationState()
    {
    }
}

public record ObjectNavigationItemViewModel(string Id, string Name, string Action, bool IsFavourite, ObjectNavigationItemViewModel[]? Items = null);

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