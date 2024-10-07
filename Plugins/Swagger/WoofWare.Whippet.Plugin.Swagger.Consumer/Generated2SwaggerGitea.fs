namespace Gitea

open WoofWare.Whippet.Plugin.Json
open WoofWare.Whippet.Plugin.HttpClient

/// Mock record type for an interface
type internal GiteaMock =
    {
        /// Returns the Person actor for a user
        ActivitypubPerson :
            string * option<System.Threading.CancellationToken> -> ActivityPub System.Threading.Tasks.Task
        /// Send to the inbox
        ActivitypubPersonInbox : string * option<System.Threading.CancellationToken> -> unit System.Threading.Tasks.Task
        /// List cron tasks
        AdminCronList : int * int * option<System.Threading.CancellationToken> -> Cron list System.Threading.Tasks.Task
        /// Run cron task
        AdminCronRun : string * option<System.Threading.CancellationToken> -> unit System.Threading.Tasks.Task
        /// List system's webhooks
        AdminListHooks : int * int * option<System.Threading.CancellationToken> -> Hook list System.Threading.Tasks.Task
        /// Create a hook
        AdminCreateHook :
            CreateHookOption * option<System.Threading.CancellationToken> -> Hook System.Threading.Tasks.Task
        /// Get a hook
        AdminGetHook : int * option<System.Threading.CancellationToken> -> Hook System.Threading.Tasks.Task
        /// Update a hook
        AdminEditHook :
            int * EditHookOption * option<System.Threading.CancellationToken> -> Hook System.Threading.Tasks.Task
        /// List all organizations
        AdminGetAllOrgs :
            int * int * option<System.Threading.CancellationToken> -> Organization list System.Threading.Tasks.Task
        /// List unadopted repositories
        AdminUnadoptedList :
            int * int * string * option<System.Threading.CancellationToken> -> string list System.Threading.Tasks.Task
        /// Adopt unadopted files as a repository
        AdminAdoptRepository :
            string * string * option<System.Threading.CancellationToken> -> unit System.Threading.Tasks.Task
        /// Delete unadopted files
        AdminDeleteUnadoptedRepository :
            string * string * option<System.Threading.CancellationToken> -> unit System.Threading.Tasks.Task
        /// List all users
        AdminGetAllUsers :
            int * int * option<System.Threading.CancellationToken> -> User list System.Threading.Tasks.Task
        /// Create a user
        AdminCreateUser :
            CreateUserOption * option<System.Threading.CancellationToken> -> User System.Threading.Tasks.Task
        /// Delete a user
        AdminDeleteUser : string * bool * option<System.Threading.CancellationToken> -> unit System.Threading.Tasks.Task
        /// Edit an existing user
        AdminEditUser :
            string * EditUserOption * option<System.Threading.CancellationToken> -> User System.Threading.Tasks.Task
        /// Add a public key on behalf of a user
        AdminCreatePublicKey :
            string * CreateKeyOption * option<System.Threading.CancellationToken>
                -> PublicKey System.Threading.Tasks.Task
        /// Delete a user's public key
        AdminDeleteUserPublicKey :
            string * int * option<System.Threading.CancellationToken> -> unit System.Threading.Tasks.Task
        /// Create an organization
        AdminCreateOrg :
            string * CreateOrgOption * option<System.Threading.CancellationToken>
                -> Organization System.Threading.Tasks.Task
        /// Create a repository on behalf of a user
        AdminCreateRepo :
            string * CreateRepoOption * option<System.Threading.CancellationToken>
                -> Repository System.Threading.Tasks.Task
        /// Delete a hook
        AdminDeleteHook : int * option<System.Threading.CancellationToken> -> unit System.Threading.Tasks.Task
        /// Render a markdown document as HTML
        RenderMarkdown :
            MarkdownOption * option<System.Threading.CancellationToken> -> string System.Threading.Tasks.Task
        /// Render raw markdown as HTML
        RenderMarkdownRaw : string * option<System.Threading.CancellationToken> -> string System.Threading.Tasks.Task
        /// Returns the nodeinfo of the Gitea application
        GetNodeInfo : option<System.Threading.CancellationToken> -> NodeInfo System.Threading.Tasks.Task
        /// List users's notification threads
        NotifyGetList :
            bool * string list * string list * string * string * int * int * option<System.Threading.CancellationToken>
                -> NotificationThread list System.Threading.Tasks.Task
        /// Mark notification threads as read, pinned or unread
        NotifyReadList :
            string * string * string list * string * option<System.Threading.CancellationToken>
                -> NotificationThread list System.Threading.Tasks.Task
        /// Check if unread notifications exist
        NotifyNewAvailable : option<System.Threading.CancellationToken> -> NotificationCount System.Threading.Tasks.Task
        /// Get notification thread by ID
        NotifyGetThread :
            string * option<System.Threading.CancellationToken> -> NotificationThread System.Threading.Tasks.Task
        /// Mark notification thread as read by ID
        NotifyReadThread :
            string * string * option<System.Threading.CancellationToken>
                -> NotificationThread System.Threading.Tasks.Task
        /// Create a repository in an organization
        CreateOrgRepoDeprecated :
            string * CreateRepoOption * option<System.Threading.CancellationToken>
                -> Repository System.Threading.Tasks.Task
        /// Get list of organizations
        OrgGetAll :
            int * int * option<System.Threading.CancellationToken> -> Organization list System.Threading.Tasks.Task
        /// Create an organization
        OrgCreate :
            CreateOrgOption * option<System.Threading.CancellationToken> -> Organization System.Threading.Tasks.Task
        /// Get an organization
        OrgGet : string * option<System.Threading.CancellationToken> -> Organization System.Threading.Tasks.Task
        /// Delete an organization
        OrgDelete : string * option<System.Threading.CancellationToken> -> unit System.Threading.Tasks.Task
        /// Edit an organization
        OrgEdit :
            string * EditOrgOption * option<System.Threading.CancellationToken>
                -> Organization System.Threading.Tasks.Task
        /// List an organization's webhooks
        OrgListHooks :
            string * int * int * option<System.Threading.CancellationToken> -> Hook list System.Threading.Tasks.Task
        /// Create a hook
        OrgCreateHook :
            string * CreateHookOption * option<System.Threading.CancellationToken> -> Hook System.Threading.Tasks.Task
        /// Get a hook
        OrgGetHook : string * int * option<System.Threading.CancellationToken> -> Hook System.Threading.Tasks.Task
        /// Delete a hook
        OrgDeleteHook : string * int * option<System.Threading.CancellationToken> -> unit System.Threading.Tasks.Task
        /// Update a hook
        OrgEditHook :
            string * int * EditHookOption * option<System.Threading.CancellationToken>
                -> Hook System.Threading.Tasks.Task
        /// List an organization's labels
        OrgListLabels :
            string * int * int * option<System.Threading.CancellationToken> -> Label list System.Threading.Tasks.Task
        /// Create a label for an organization
        OrgCreateLabel :
            string * CreateLabelOption * option<System.Threading.CancellationToken> -> Label System.Threading.Tasks.Task
        /// Get a single label
        OrgGetLabel : string * int * option<System.Threading.CancellationToken> -> Label System.Threading.Tasks.Task
        /// Delete a label
        OrgDeleteLabel : string * int * option<System.Threading.CancellationToken> -> unit System.Threading.Tasks.Task
        /// Update a label
        OrgEditLabel :
            string * int * EditLabelOption * option<System.Threading.CancellationToken>
                -> Label System.Threading.Tasks.Task
        /// List an organization's members
        OrgListMembers :
            string * int * int * option<System.Threading.CancellationToken> -> User list System.Threading.Tasks.Task
        /// Check if a user is a member of an organization
        OrgIsMember : string * string * option<System.Threading.CancellationToken> -> unit System.Threading.Tasks.Task
        /// Remove a member from an organization
        OrgDeleteMember :
            string * string * option<System.Threading.CancellationToken> -> unit System.Threading.Tasks.Task
        /// List an organization's public members
        OrgListPublicMembers :
            string * int * int * option<System.Threading.CancellationToken> -> User list System.Threading.Tasks.Task
        /// Check if a user is a public member of an organization
        OrgIsPublicMember :
            string * string * option<System.Threading.CancellationToken> -> unit System.Threading.Tasks.Task
        /// Conceal a user's membership
        OrgConcealMember :
            string * string * option<System.Threading.CancellationToken> -> unit System.Threading.Tasks.Task
        /// Publicize a user's membership
        OrgPublicizeMember :
            string * string * option<System.Threading.CancellationToken> -> unit System.Threading.Tasks.Task
        /// List an organization's repos
        OrgListRepos :
            string * int * int * option<System.Threading.CancellationToken>
                -> Repository list System.Threading.Tasks.Task
        /// Create a repository in an organization
        CreateOrgRepo :
            string * CreateRepoOption * option<System.Threading.CancellationToken>
                -> Repository System.Threading.Tasks.Task
        /// List an organization's teams
        OrgListTeams :
            string * int * int * option<System.Threading.CancellationToken> -> Team list System.Threading.Tasks.Task
        /// Create a team
        OrgCreateTeam :
            string * CreateTeamOption * option<System.Threading.CancellationToken> -> Team System.Threading.Tasks.Task
        /// Search for teams within an organization
        TeamSearch :
            string * string * bool * int * int * option<System.Threading.CancellationToken>
                -> unit System.Threading.Tasks.Task
        /// Gets all packages of an owner
        ListPackages :
            string * int * int * string * string * option<System.Threading.CancellationToken>
                -> Package list System.Threading.Tasks.Task
        /// Gets a package
        GetPackage :
            string * string * string * string * option<System.Threading.CancellationToken>
                -> Package System.Threading.Tasks.Task
        /// Delete a package
        DeletePackage :
            string * string * string * string * option<System.Threading.CancellationToken>
                -> unit System.Threading.Tasks.Task
        /// Gets all files of a package
        ListPackageFiles :
            string * string * string * string * option<System.Threading.CancellationToken>
                -> PackageFile list System.Threading.Tasks.Task
        /// Search for issues across the repositories that the user has access to
        IssueSearchIssues :
            string *
            string *
            string *
            string *
            int *
            string *
            string *
            string *
            bool *
            bool *
            bool *
            bool *
            string *
            string *
            int *
            int *
            option<System.Threading.CancellationToken>
                -> Issue list System.Threading.Tasks.Task
        /// Migrate a remote git repository
        RepoMigrate :
            MigrateRepoOptions * option<System.Threading.CancellationToken> -> Repository System.Threading.Tasks.Task
        /// Search for repositories
        RepoSearch :
            string *
            bool *
            bool *
            int *
            int *
            int *
            int *
            bool *
            bool *
            bool *
            bool *
            string *
            bool *
            string *
            string *
            int *
            int *
            option<System.Threading.CancellationToken>
                -> SearchResults System.Threading.Tasks.Task
        /// Get a repository
        RepoGet : string * string * option<System.Threading.CancellationToken> -> Repository System.Threading.Tasks.Task
        /// Delete a repository
        RepoDelete : string * string * option<System.Threading.CancellationToken> -> unit System.Threading.Tasks.Task
        /// Edit a repository's properties. Only fields that are set will be changed.
        RepoEdit :
            string * string * EditRepoOption * option<System.Threading.CancellationToken>
                -> Repository System.Threading.Tasks.Task
        /// Get an archive of a repository
        RepoGetArchive :
            string * string * string * option<System.Threading.CancellationToken> -> unit System.Threading.Tasks.Task
        /// Return all users that have write access and can be assigned to issues
        RepoGetAssignees :
            string * string * option<System.Threading.CancellationToken> -> User list System.Threading.Tasks.Task
        /// List branch protections for a repository
        RepoListBranchProtection :
            string * string * option<System.Threading.CancellationToken>
                -> BranchProtection list System.Threading.Tasks.Task
        /// Create a branch protections for a repository
        RepoCreateBranchProtection :
            string * string * CreateBranchProtectionOption * option<System.Threading.CancellationToken>
                -> BranchProtection System.Threading.Tasks.Task
        /// Get a specific branch protection for the repository
        RepoGetBranchProtection :
            string * string * string * option<System.Threading.CancellationToken>
                -> BranchProtection System.Threading.Tasks.Task
        /// Delete a specific branch protection for the repository
        RepoDeleteBranchProtection :
            string * string * string * option<System.Threading.CancellationToken> -> unit System.Threading.Tasks.Task
        /// Edit a branch protections for a repository. Only fields that are set will be changed
        RepoEditBranchProtection :
            string * string * string * EditBranchProtectionOption * option<System.Threading.CancellationToken>
                -> BranchProtection System.Threading.Tasks.Task
        /// List a repository's branches
        RepoListBranches :
            string * string * int * int * option<System.Threading.CancellationToken>
                -> Branch list System.Threading.Tasks.Task
        /// Create a branch
        RepoCreateBranch :
            string * string * CreateBranchRepoOption * option<System.Threading.CancellationToken>
                -> Branch System.Threading.Tasks.Task
        /// Retrieve a specific branch from a repository, including its effective branch protection
        RepoGetBranch :
            string * string * string * option<System.Threading.CancellationToken> -> Branch System.Threading.Tasks.Task
        /// Delete a specific branch from a repository
        RepoDeleteBranch :
            string * string * string * option<System.Threading.CancellationToken> -> unit System.Threading.Tasks.Task
        /// List a repository's collaborators
        RepoListCollaborators :
            string * string * int * int * option<System.Threading.CancellationToken>
                -> User list System.Threading.Tasks.Task
        /// Check if a user is a collaborator of a repository
        RepoCheckCollaborator :
            string * string * string * option<System.Threading.CancellationToken> -> unit System.Threading.Tasks.Task
        /// Delete a collaborator from a repository
        RepoDeleteCollaborator :
            string * string * string * option<System.Threading.CancellationToken> -> unit System.Threading.Tasks.Task
        /// Add a collaborator to a repository
        RepoAddCollaborator :
            string * string * string * AddCollaboratorOption * option<System.Threading.CancellationToken>
                -> unit System.Threading.Tasks.Task
        /// Get repository permissions for a user
        RepoGetRepoPermissions :
            string * string * string * option<System.Threading.CancellationToken>
                -> RepoCollaboratorPermission System.Threading.Tasks.Task
        /// Get a list of all commits from a repository
        RepoGetAllCommits :
            string * string * string * string * bool * int * int * option<System.Threading.CancellationToken>
                -> Commit list System.Threading.Tasks.Task
        /// Get a commit's combined status, by branch/tag/commit reference
        RepoGetCombinedStatusByRef :
            string * string * string * int * int * option<System.Threading.CancellationToken>
                -> CombinedStatus System.Threading.Tasks.Task
        /// Get a commit's statuses, by branch/tag/commit reference
        RepoListStatusesByRef :
            string * string * string * string * string * int * int * option<System.Threading.CancellationToken>
                -> CommitStatus list System.Threading.Tasks.Task
        /// Gets the metadata of all the entries of the root dir
        RepoGetContentsList :
            string * string * string * option<System.Threading.CancellationToken>
                -> ContentsResponse list System.Threading.Tasks.Task
        /// Gets the metadata and contents (if a file) of an entry in a repository, or a list of entries if a dir
        RepoGetContents :
            string * string * string * string * option<System.Threading.CancellationToken>
                -> ContentsResponse System.Threading.Tasks.Task
        /// Create a file in a repository
        RepoCreateFile :
            string * string * string * CreateFileOptions * option<System.Threading.CancellationToken>
                -> FileResponse System.Threading.Tasks.Task
        /// Delete a file in a repository
        RepoDeleteFile :
            string * string * string * DeleteFileOptions * option<System.Threading.CancellationToken>
                -> FileDeleteResponse System.Threading.Tasks.Task
        /// Update a file in a repository
        RepoUpdateFile :
            string * string * string * UpdateFileOptions * option<System.Threading.CancellationToken>
                -> FileResponse System.Threading.Tasks.Task
        /// Apply diff patch to repository
        RepoApplyDiffPatch :
            string * string * UpdateFileOptions * option<System.Threading.CancellationToken>
                -> FileResponse System.Threading.Tasks.Task
        /// Get the EditorConfig definitions of a file in a repository
        RepoGetEditorConfig :
            string * string * string * string * option<System.Threading.CancellationToken>
                -> unit System.Threading.Tasks.Task
        /// List a repository's forks
        ListForks :
            string * string * int * int * option<System.Threading.CancellationToken>
                -> Repository list System.Threading.Tasks.Task
        /// Fork a repository
        CreateFork :
            string * string * CreateForkOption * option<System.Threading.CancellationToken>
                -> Repository System.Threading.Tasks.Task
        /// Gets the blob of a repository.
        GetBlob :
            string * string * string * option<System.Threading.CancellationToken>
                -> GitBlobResponse System.Threading.Tasks.Task
        /// Get a single commit from a repository
        RepoGetSingleCommit :
            string * string * string * option<System.Threading.CancellationToken> -> Commit System.Threading.Tasks.Task
        /// Get a commit's diff or patch
        RepoDownloadCommitDiffOrPatch :
            string * string * string * string * option<System.Threading.CancellationToken>
                -> string System.Threading.Tasks.Task
        /// Get a note corresponding to a single commit from a repository
        RepoGetNote :
            string * string * string * option<System.Threading.CancellationToken> -> Note System.Threading.Tasks.Task
        /// Get specified ref or filtered repository's refs
        RepoListAllGitRefs :
            string * string * option<System.Threading.CancellationToken> -> Reference list System.Threading.Tasks.Task
        /// Get specified ref or filtered repository's refs
        RepoListGitRefs :
            string * string * string * option<System.Threading.CancellationToken>
                -> Reference list System.Threading.Tasks.Task
        /// Gets the tag object of an annotated tag (not lightweight tags)
        GetAnnotatedTag :
            string * string * string * option<System.Threading.CancellationToken>
                -> AnnotatedTag System.Threading.Tasks.Task
        /// Gets the tree of a repository.
        GetTree :
            string * string * string * bool * int * int * option<System.Threading.CancellationToken>
                -> GitTreeResponse System.Threading.Tasks.Task
        /// List the hooks in a repository
        RepoListHooks :
            string * string * int * int * option<System.Threading.CancellationToken>
                -> Hook list System.Threading.Tasks.Task
        /// Create a hook
        RepoCreateHook :
            string * string * CreateHookOption * option<System.Threading.CancellationToken>
                -> Hook System.Threading.Tasks.Task
        /// List the Git hooks in a repository
        RepoListGitHooks :
            string * string * option<System.Threading.CancellationToken> -> GitHook list System.Threading.Tasks.Task
        /// Get a Git hook
        RepoGetGitHook :
            string * string * string * option<System.Threading.CancellationToken> -> GitHook System.Threading.Tasks.Task
        /// Delete a Git hook in a repository
        RepoDeleteGitHook :
            string * string * string * option<System.Threading.CancellationToken> -> unit System.Threading.Tasks.Task
        /// Edit a Git hook in a repository
        RepoEditGitHook :
            string * string * string * EditGitHookOption * option<System.Threading.CancellationToken>
                -> GitHook System.Threading.Tasks.Task
        /// Get a hook
        RepoGetHook :
            string * string * int * option<System.Threading.CancellationToken> -> Hook System.Threading.Tasks.Task
        /// Delete a hook in a repository
        RepoDeleteHook :
            string * string * int * option<System.Threading.CancellationToken> -> unit System.Threading.Tasks.Task
        /// Edit a hook in a repository
        RepoEditHook :
            string * string * int * EditHookOption * option<System.Threading.CancellationToken>
                -> Hook System.Threading.Tasks.Task
        /// Test a push webhook
        RepoTestHook :
            string * string * int * string * option<System.Threading.CancellationToken>
                -> unit System.Threading.Tasks.Task
        /// Get available issue templates for a repository
        RepoGetIssueTemplates :
            string * string * option<System.Threading.CancellationToken>
                -> IssueTemplate list System.Threading.Tasks.Task
        /// List a repository's issues
        IssueListIssues :
            string *
            string *
            string *
            string *
            string *
            string *
            string *
            string *
            string *
            string *
            string *
            string *
            int *
            int *
            option<System.Threading.CancellationToken>
                -> Issue list System.Threading.Tasks.Task
        /// Create an issue. If using deadline only the date will be taken into account, and time of day ignored.
        IssueCreateIssue :
            string * string * CreateIssueOption * option<System.Threading.CancellationToken>
                -> Issue System.Threading.Tasks.Task
        /// List all comments in a repository
        IssueGetRepoComments :
            string * string * string * string * int * int * option<System.Threading.CancellationToken>
                -> Comment list System.Threading.Tasks.Task
        /// Delete a comment
        IssueDeleteComment :
            string * string * int * option<System.Threading.CancellationToken> -> unit System.Threading.Tasks.Task
        /// List comment's attachments
        IssueListIssueCommentAttachments :
            string * string * int * option<System.Threading.CancellationToken>
                -> Attachment list System.Threading.Tasks.Task
        /// Get a comment attachment
        IssueGetIssueCommentAttachment :
            string * string * int * int * option<System.Threading.CancellationToken>
                -> Attachment System.Threading.Tasks.Task
        /// Delete a comment attachment
        IssueDeleteIssueCommentAttachment :
            string * string * int * int * option<System.Threading.CancellationToken> -> unit System.Threading.Tasks.Task
        /// Edit a comment attachment
        IssueEditIssueCommentAttachment :
            string * string * int * int * EditAttachmentOptions * option<System.Threading.CancellationToken>
                -> Attachment System.Threading.Tasks.Task
        /// Get a list of reactions from a comment of an issue
        IssueGetCommentReactions :
            string * string * int * option<System.Threading.CancellationToken>
                -> Reaction list System.Threading.Tasks.Task
        /// Remove a reaction from a comment of an issue
        IssueDeleteCommentReaction :
            string * string * int * EditReactionOption * option<System.Threading.CancellationToken>
                -> unit System.Threading.Tasks.Task
        /// Get an issue
        IssueGetIssue :
            string * string * int * option<System.Threading.CancellationToken> -> Issue System.Threading.Tasks.Task
        /// Delete an issue
        IssueDelete :
            string * string * int * option<System.Threading.CancellationToken> -> unit System.Threading.Tasks.Task
        /// Edit an issue. If using deadline only the date will be taken into account, and time of day ignored.
        IssueEditIssue :
            string * string * int * EditIssueOption * option<System.Threading.CancellationToken>
                -> Issue System.Threading.Tasks.Task
        /// List issue's attachments
        IssueListIssueAttachments :
            string * string * int * option<System.Threading.CancellationToken>
                -> Attachment list System.Threading.Tasks.Task
        /// Get an issue attachment
        IssueGetIssueAttachment :
            string * string * int * int * option<System.Threading.CancellationToken>
                -> Attachment System.Threading.Tasks.Task
        /// Delete an issue attachment
        IssueDeleteIssueAttachment :
            string * string * int * int * option<System.Threading.CancellationToken> -> unit System.Threading.Tasks.Task
        /// Edit an issue attachment
        IssueEditIssueAttachment :
            string * string * int * int * EditAttachmentOptions * option<System.Threading.CancellationToken>
                -> Attachment System.Threading.Tasks.Task
        /// List all comments on an issue
        IssueGetComments :
            string * string * int * string * string * option<System.Threading.CancellationToken>
                -> Comment list System.Threading.Tasks.Task
        /// Add a comment to an issue
        IssueCreateComment :
            string * string * int * CreateIssueCommentOption * option<System.Threading.CancellationToken>
                -> Comment System.Threading.Tasks.Task
        /// Delete a comment
        IssueDeleteCommentDeprecated :
            string * string * int * int * option<System.Threading.CancellationToken> -> unit System.Threading.Tasks.Task
        /// Set an issue deadline. If set to null, the deadline is deleted. If using deadline only the date will be taken into account, and time of day ignored.
        IssueEditIssueDeadline :
            string * string * int * EditDeadlineOption * option<System.Threading.CancellationToken>
                -> IssueDeadline System.Threading.Tasks.Task
        /// Get an issue's labels
        IssueGetLabels :
            string * string * int * option<System.Threading.CancellationToken> -> Label list System.Threading.Tasks.Task
        /// Add a label to an issue
        IssueAddLabel :
            string * string * int * IssueLabelsOption * option<System.Threading.CancellationToken>
                -> Label list System.Threading.Tasks.Task
        /// Remove all labels from an issue
        IssueClearLabels :
            string * string * int * option<System.Threading.CancellationToken> -> unit System.Threading.Tasks.Task
        /// Replace an issue's labels
        IssueReplaceLabels :
            string * string * int * IssueLabelsOption * option<System.Threading.CancellationToken>
                -> Label list System.Threading.Tasks.Task
        /// Remove a label from an issue
        IssueRemoveLabel :
            string * string * int * int * option<System.Threading.CancellationToken> -> unit System.Threading.Tasks.Task
        /// Get a list reactions of an issue
        IssueGetIssueReactions :
            string * string * int * int * int * option<System.Threading.CancellationToken>
                -> Reaction list System.Threading.Tasks.Task
        /// Remove a reaction from an issue
        IssueDeleteIssueReaction :
            string * string * int * EditReactionOption * option<System.Threading.CancellationToken>
                -> unit System.Threading.Tasks.Task
        /// Delete an issue's existing stopwatch.
        IssueDeleteStopWatch :
            string * string * int * option<System.Threading.CancellationToken> -> unit System.Threading.Tasks.Task
        /// Start stopwatch on an issue.
        IssueStartStopWatch :
            string * string * int * option<System.Threading.CancellationToken> -> unit System.Threading.Tasks.Task
        /// Stop an issue's existing stopwatch.
        IssueStopStopWatch :
            string * string * int * option<System.Threading.CancellationToken> -> unit System.Threading.Tasks.Task
        /// Get users who subscribed on an issue.
        IssueSubscriptions :
            string * string * int * int * int * option<System.Threading.CancellationToken>
                -> User list System.Threading.Tasks.Task
        /// Check if user is subscribed to an issue
        IssueCheckSubscription :
            string * string * int * option<System.Threading.CancellationToken> -> WatchInfo System.Threading.Tasks.Task
        /// List all comments and events on an issue
        IssueGetCommentsAndTimeline :
            string * string * int * string * int * int * string * option<System.Threading.CancellationToken>
                -> TimelineComment list System.Threading.Tasks.Task
        /// List an issue's tracked times
        IssueTrackedTimes :
            string * string * int * string * string * string * int * int * option<System.Threading.CancellationToken>
                -> TrackedTime list System.Threading.Tasks.Task
        /// Add tracked time to a issue
        IssueAddTime :
            string * string * int * AddTimeOption * option<System.Threading.CancellationToken>
                -> TrackedTime System.Threading.Tasks.Task
        /// Reset a tracked time of an issue
        IssueResetTime :
            string * string * int * option<System.Threading.CancellationToken> -> unit System.Threading.Tasks.Task
        /// Delete specific tracked time
        IssueDeleteTime :
            string * string * int * int * option<System.Threading.CancellationToken> -> unit System.Threading.Tasks.Task
        /// List a repository's keys
        RepoListKeys :
            string * string * int * string * int * int * option<System.Threading.CancellationToken>
                -> DeployKey list System.Threading.Tasks.Task
        /// Add a key to a repository
        RepoCreateKey :
            string * string * CreateKeyOption * option<System.Threading.CancellationToken>
                -> DeployKey System.Threading.Tasks.Task
        /// Get a repository's key by id
        RepoGetKey :
            string * string * int * option<System.Threading.CancellationToken> -> DeployKey System.Threading.Tasks.Task
        /// Delete a key from a repository
        RepoDeleteKey :
            string * string * int * option<System.Threading.CancellationToken> -> unit System.Threading.Tasks.Task
        /// Get all of a repository's labels
        IssueListLabels :
            string * string * int * int * option<System.Threading.CancellationToken>
                -> Label list System.Threading.Tasks.Task
        /// Create a label
        IssueCreateLabel :
            string * string * CreateLabelOption * option<System.Threading.CancellationToken>
                -> Label System.Threading.Tasks.Task
        /// Get a single label
        IssueGetLabel :
            string * string * int * option<System.Threading.CancellationToken> -> Label System.Threading.Tasks.Task
        /// Delete a label
        IssueDeleteLabel :
            string * string * int * option<System.Threading.CancellationToken> -> unit System.Threading.Tasks.Task
        /// Update a label
        IssueEditLabel :
            string * string * int * EditLabelOption * option<System.Threading.CancellationToken>
                -> Label System.Threading.Tasks.Task
        /// Get languages and number of bytes of code written
        RepoGetLanguages :
            string * string * option<System.Threading.CancellationToken>
                -> LanguageStatistics System.Threading.Tasks.Task
        /// Get a file or it's LFS object from a repository
        RepoGetRawFileOrLFS :
            string * string * string * string * option<System.Threading.CancellationToken>
                -> unit System.Threading.Tasks.Task
        /// Get all of a repository's opened milestones
        IssueGetMilestonesList :
            string * string * string * string * int * int * option<System.Threading.CancellationToken>
                -> Milestone list System.Threading.Tasks.Task
        /// Create a milestone
        IssueCreateMilestone :
            string * string * CreateMilestoneOption * option<System.Threading.CancellationToken>
                -> Milestone System.Threading.Tasks.Task
        /// Get a milestone
        IssueGetMilestone :
            string * string * string * option<System.Threading.CancellationToken>
                -> Milestone System.Threading.Tasks.Task
        /// Delete a milestone
        IssueDeleteMilestone :
            string * string * string * option<System.Threading.CancellationToken> -> unit System.Threading.Tasks.Task
        /// Update a milestone
        IssueEditMilestone :
            string * string * string * EditMilestoneOption * option<System.Threading.CancellationToken>
                -> Milestone System.Threading.Tasks.Task
        /// Sync a mirrored repository
        RepoMirrorSync :
            string * string * option<System.Threading.CancellationToken> -> unit System.Threading.Tasks.Task
        /// List users's notification threads on a specific repo
        NotifyGetRepoList :
            string *
            string *
            bool *
            string list *
            string list *
            string *
            string *
            int *
            int *
            option<System.Threading.CancellationToken>
                -> NotificationThread list System.Threading.Tasks.Task
        /// Mark notification threads as read, pinned or unread on a specific repo
        NotifyReadRepoList :
            string * string * string * string list * string * string * option<System.Threading.CancellationToken>
                -> NotificationThread list System.Threading.Tasks.Task
        /// List a repo's pull requests
        RepoListPullRequests :
            string * string * string * string * int * int list * int * int * option<System.Threading.CancellationToken>
                -> PullRequest list System.Threading.Tasks.Task
        /// Create a pull request
        RepoCreatePullRequest :
            string * string * CreatePullRequestOption * option<System.Threading.CancellationToken>
                -> PullRequest System.Threading.Tasks.Task
        /// Get a pull request
        RepoGetPullRequest :
            string * string * int * option<System.Threading.CancellationToken>
                -> PullRequest System.Threading.Tasks.Task
        /// Update a pull request. If using deadline only the date will be taken into account, and time of day ignored.
        RepoEditPullRequest :
            string * string * int * EditPullRequestOption * option<System.Threading.CancellationToken>
                -> PullRequest System.Threading.Tasks.Task
        /// Get a pull request diff or patch
        RepoDownloadPullDiffOrPatch :
            string * string * int * string * bool * option<System.Threading.CancellationToken>
                -> string System.Threading.Tasks.Task
        /// Get commits for a pull request
        RepoGetPullRequestCommits :
            string * string * int * int * int * option<System.Threading.CancellationToken>
                -> Commit list System.Threading.Tasks.Task
        /// Get changed files for a pull request
        RepoGetPullRequestFiles :
            string * string * int * string * string * int * int * option<System.Threading.CancellationToken>
                -> ChangedFile list System.Threading.Tasks.Task
        /// Check if a pull request has been merged
        RepoPullRequestIsMerged :
            string * string * int * option<System.Threading.CancellationToken> -> unit System.Threading.Tasks.Task
        /// Merge a pull request
        RepoMergePullRequest :
            string * string * int * MergePullRequestOption * option<System.Threading.CancellationToken>
                -> unit System.Threading.Tasks.Task
        /// Cancel the scheduled auto merge for the given pull request
        RepoCancelScheduledAutoMerge :
            string * string * int * option<System.Threading.CancellationToken> -> unit System.Threading.Tasks.Task
        /// create review requests for a pull request
        RepoCreatePullReviewRequests :
            string * string * int * PullReviewRequestOptions * option<System.Threading.CancellationToken>
                -> PullReview list System.Threading.Tasks.Task
        /// cancel review requests for a pull request
        RepoDeletePullReviewRequests :
            string * string * int * PullReviewRequestOptions * option<System.Threading.CancellationToken>
                -> unit System.Threading.Tasks.Task
        /// List all reviews for a pull request
        RepoListPullReviews :
            string * string * int * int * int * option<System.Threading.CancellationToken>
                -> PullReview list System.Threading.Tasks.Task
        /// Create a review to an pull request
        RepoCreatePullReview :
            string * string * int * CreatePullReviewOptions * option<System.Threading.CancellationToken>
                -> PullReview System.Threading.Tasks.Task
        /// Get a specific review for a pull request
        RepoGetPullReview :
            string * string * int * int * option<System.Threading.CancellationToken>
                -> PullReview System.Threading.Tasks.Task
        /// Submit a pending review to an pull request
        RepoSubmitPullReview :
            string * string * int * int * SubmitPullReviewOptions * option<System.Threading.CancellationToken>
                -> PullReview System.Threading.Tasks.Task
        /// Delete a specific review from a pull request
        RepoDeletePullReview :
            string * string * int * int * option<System.Threading.CancellationToken> -> unit System.Threading.Tasks.Task
        /// Get a specific review for a pull request
        RepoGetPullReviewComments :
            string * string * int * int * option<System.Threading.CancellationToken>
                -> PullReviewComment list System.Threading.Tasks.Task
        /// Dismiss a review for a pull request
        RepoDismissPullReview :
            string * string * int * int * DismissPullReviewOptions * option<System.Threading.CancellationToken>
                -> PullReview System.Threading.Tasks.Task
        /// Cancel to dismiss a review for a pull request
        RepoUnDismissPullReview :
            string * string * int * int * option<System.Threading.CancellationToken>
                -> PullReview System.Threading.Tasks.Task
        /// Merge PR's baseBranch into headBranch
        RepoUpdatePullRequest :
            string * string * int * string * option<System.Threading.CancellationToken>
                -> unit System.Threading.Tasks.Task
        /// Get all push mirrors of the repository
        RepoListPushMirrors :
            string * string * int * int * option<System.Threading.CancellationToken>
                -> PushMirror list System.Threading.Tasks.Task
        /// add a push mirror to the repository
        RepoAddPushMirror :
            string * string * CreatePushMirrorOption * option<System.Threading.CancellationToken>
                -> PushMirror System.Threading.Tasks.Task
        /// Sync all push mirrored repository
        RepoPushMirrorSync :
            string * string * option<System.Threading.CancellationToken> -> unit System.Threading.Tasks.Task
        /// Get push mirror of the repository by remoteName
        RepoGetPushMirrorByRemoteName :
            string * string * string * option<System.Threading.CancellationToken>
                -> PushMirror System.Threading.Tasks.Task
        /// deletes a push mirror from a repository by remoteName
        RepoDeletePushMirror :
            string * string * string * option<System.Threading.CancellationToken> -> unit System.Threading.Tasks.Task
        /// Get a file from a repository
        RepoGetRawFile :
            string * string * string * string * option<System.Threading.CancellationToken>
                -> unit System.Threading.Tasks.Task
        /// List a repo's releases
        RepoListReleases :
            string * string * bool * bool * int * int * int * option<System.Threading.CancellationToken>
                -> Release list System.Threading.Tasks.Task
        /// Create a release
        RepoCreateRelease :
            string * string * CreateReleaseOption * option<System.Threading.CancellationToken>
                -> Release System.Threading.Tasks.Task
        /// Gets the most recent non-prerelease, non-draft release of a repository, sorted by created_at
        RepoGetLatestRelease :
            string * string * option<System.Threading.CancellationToken> -> Release System.Threading.Tasks.Task
        /// Get a release by tag name
        RepoGetReleaseByTag :
            string * string * string * option<System.Threading.CancellationToken> -> Release System.Threading.Tasks.Task
        /// Delete a release by tag name
        RepoDeleteReleaseByTag :
            string * string * string * option<System.Threading.CancellationToken> -> unit System.Threading.Tasks.Task
        /// Get a release
        RepoGetRelease :
            string * string * int * option<System.Threading.CancellationToken> -> Release System.Threading.Tasks.Task
        /// Delete a release
        RepoDeleteRelease :
            string * string * int * option<System.Threading.CancellationToken> -> unit System.Threading.Tasks.Task
        /// Update a release
        RepoEditRelease :
            string * string * int * EditReleaseOption * option<System.Threading.CancellationToken>
                -> Release System.Threading.Tasks.Task
        /// List release's attachments
        RepoListReleaseAttachments :
            string * string * int * option<System.Threading.CancellationToken>
                -> Attachment list System.Threading.Tasks.Task
        /// Get a release attachment
        RepoGetReleaseAttachment :
            string * string * int * int * option<System.Threading.CancellationToken>
                -> Attachment System.Threading.Tasks.Task
        /// Delete a release attachment
        RepoDeleteReleaseAttachment :
            string * string * int * int * option<System.Threading.CancellationToken> -> unit System.Threading.Tasks.Task
        /// Edit a release attachment
        RepoEditReleaseAttachment :
            string * string * int * int * EditAttachmentOptions * option<System.Threading.CancellationToken>
                -> Attachment System.Threading.Tasks.Task
        /// Return all users that can be requested to review in this repo
        RepoGetReviewers :
            string * string * option<System.Threading.CancellationToken> -> User list System.Threading.Tasks.Task
        /// Get signing-key.gpg for given repository
        RepoSigningKey :
            string * string * option<System.Threading.CancellationToken> -> unit System.Threading.Tasks.Task
        /// List a repo's stargazers
        RepoListStargazers :
            string * string * int * int * option<System.Threading.CancellationToken>
                -> User list System.Threading.Tasks.Task
        /// Get a commit's statuses
        RepoListStatuses :
            string * string * string * string * string * int * int * option<System.Threading.CancellationToken>
                -> CommitStatus list System.Threading.Tasks.Task
        /// Create a commit status
        RepoCreateStatus :
            string * string * string * CreateStatusOption * option<System.Threading.CancellationToken>
                -> CommitStatus System.Threading.Tasks.Task
        /// List a repo's watchers
        RepoListSubscribers :
            string * string * int * int * option<System.Threading.CancellationToken>
                -> User list System.Threading.Tasks.Task
        /// Check if the current user is watching a repo
        UserCurrentCheckSubscription :
            string * string * option<System.Threading.CancellationToken> -> WatchInfo System.Threading.Tasks.Task
        /// Unwatch a repo
        UserCurrentDeleteSubscription :
            string * string * option<System.Threading.CancellationToken> -> unit System.Threading.Tasks.Task
        /// Watch a repo
        UserCurrentPutSubscription :
            string * string * option<System.Threading.CancellationToken> -> WatchInfo System.Threading.Tasks.Task
        /// List a repository's tags
        RepoListTags :
            string * string * int * int * option<System.Threading.CancellationToken>
                -> Tag list System.Threading.Tasks.Task
        /// Create a new git tag in a repository
        RepoCreateTag :
            string * string * CreateTagOption * option<System.Threading.CancellationToken>
                -> Tag System.Threading.Tasks.Task
        /// Get the tag of a repository by tag name
        RepoGetTag :
            string * string * string * option<System.Threading.CancellationToken> -> Tag System.Threading.Tasks.Task
        /// Delete a repository's tag by name
        RepoDeleteTag :
            string * string * string * option<System.Threading.CancellationToken> -> unit System.Threading.Tasks.Task
        /// List a repository's teams
        RepoListTeams :
            string * string * option<System.Threading.CancellationToken> -> Team list System.Threading.Tasks.Task
        /// Check if a team is assigned to a repository
        RepoCheckTeam :
            string * string * string * option<System.Threading.CancellationToken> -> Team System.Threading.Tasks.Task
        /// Delete a team from a repository
        RepoDeleteTeam :
            string * string * string * option<System.Threading.CancellationToken> -> unit System.Threading.Tasks.Task
        /// Add a team to a repository
        RepoAddTeam :
            string * string * string * option<System.Threading.CancellationToken> -> unit System.Threading.Tasks.Task
        /// List a repo's tracked times
        RepoTrackedTimes :
            string * string * string * string * string * int * int * option<System.Threading.CancellationToken>
                -> TrackedTime list System.Threading.Tasks.Task
        /// List a user's tracked times in a repo
        UserTrackedTimes :
            string * string * string * option<System.Threading.CancellationToken>
                -> TrackedTime list System.Threading.Tasks.Task
        /// Get list of topics that a repository has
        RepoListTopics :
            string * string * int * int * option<System.Threading.CancellationToken>
                -> TopicName System.Threading.Tasks.Task
        /// Replace list of topics for a repository
        RepoUpdateTopics :
            string * string * RepoTopicOptions * option<System.Threading.CancellationToken>
                -> unit System.Threading.Tasks.Task
        /// Delete a topic from a repository
        RepoDeleteTopic :
            string * string * string * option<System.Threading.CancellationToken> -> unit System.Threading.Tasks.Task
        /// Add a topic to a repository
        RepoAddTopic :
            string * string * string * option<System.Threading.CancellationToken> -> unit System.Threading.Tasks.Task
        /// Transfer a repo ownership
        RepoTransfer :
            string * string * TransferRepoOption * option<System.Threading.CancellationToken>
                -> Repository System.Threading.Tasks.Task
        /// Accept a repo transfer
        AcceptRepoTransfer :
            string * string * option<System.Threading.CancellationToken> -> Repository System.Threading.Tasks.Task
        /// Reject a repo transfer
        RejectRepoTransfer :
            string * string * option<System.Threading.CancellationToken> -> Repository System.Threading.Tasks.Task
        /// Create a wiki page
        RepoCreateWikiPage :
            string * string * CreateWikiPageOptions * option<System.Threading.CancellationToken>
                -> WikiPage System.Threading.Tasks.Task
        /// Get a wiki page
        RepoGetWikiPage :
            string * string * string * option<System.Threading.CancellationToken>
                -> WikiPage System.Threading.Tasks.Task
        /// Delete a wiki page
        RepoDeleteWikiPage :
            string * string * string * option<System.Threading.CancellationToken> -> unit System.Threading.Tasks.Task
        /// Edit a wiki page
        RepoEditWikiPage :
            string * string * string * CreateWikiPageOptions * option<System.Threading.CancellationToken>
                -> WikiPage System.Threading.Tasks.Task
        /// Get all wiki pages
        RepoGetWikiPages :
            string * string * int * int * option<System.Threading.CancellationToken>
                -> WikiPageMetaData list System.Threading.Tasks.Task
        /// Get revisions of a wiki page
        RepoGetWikiPageRevisions :
            string * string * string * int * option<System.Threading.CancellationToken>
                -> WikiCommitList System.Threading.Tasks.Task
        /// Create a repository using a template
        GenerateRepo :
            string * string * GenerateRepoOption * option<System.Threading.CancellationToken>
                -> Repository System.Threading.Tasks.Task
        /// Get a repository by id
        RepoGetByID : int * option<System.Threading.CancellationToken> -> Repository System.Threading.Tasks.Task
        /// Get instance's global settings for api
        GetGeneralAPISettings :
            option<System.Threading.CancellationToken> -> GeneralAPISettings System.Threading.Tasks.Task
        /// Get instance's global settings for Attachment
        GetGeneralAttachmentSettings :
            option<System.Threading.CancellationToken> -> GeneralAttachmentSettings System.Threading.Tasks.Task
        /// Get instance's global settings for repositories
        GetGeneralRepositorySettings :
            option<System.Threading.CancellationToken> -> GeneralRepoSettings System.Threading.Tasks.Task
        /// Get instance's global settings for ui
        GetGeneralUISettings :
            option<System.Threading.CancellationToken> -> GeneralUISettings System.Threading.Tasks.Task
        /// Get default signing-key.gpg
        GetSigningKey : option<System.Threading.CancellationToken> -> unit System.Threading.Tasks.Task
        /// Get a team
        OrgGetTeam : int * option<System.Threading.CancellationToken> -> Team System.Threading.Tasks.Task
        /// Delete a team
        OrgDeleteTeam : int * option<System.Threading.CancellationToken> -> unit System.Threading.Tasks.Task
        /// Edit a team
        OrgEditTeam :
            int * EditTeamOption * option<System.Threading.CancellationToken> -> Team System.Threading.Tasks.Task
        /// List a team's members
        OrgListTeamMembers :
            int * int * int * option<System.Threading.CancellationToken> -> User list System.Threading.Tasks.Task
        /// List a particular member of team
        OrgListTeamMember :
            int * string * option<System.Threading.CancellationToken> -> User System.Threading.Tasks.Task
        /// Remove a team member
        OrgRemoveTeamMember :
            int * string * option<System.Threading.CancellationToken> -> unit System.Threading.Tasks.Task
        /// Add a team member
        OrgAddTeamMember : int * string * option<System.Threading.CancellationToken> -> unit System.Threading.Tasks.Task
        /// List a team's repos
        OrgListTeamRepos :
            int * int * int * option<System.Threading.CancellationToken> -> Repository list System.Threading.Tasks.Task
        /// List a particular repo of team
        OrgListTeamRepo :
            int * string * string * option<System.Threading.CancellationToken> -> Repository System.Threading.Tasks.Task
        /// Remove a repository from a team
        OrgRemoveTeamRepository :
            int * string * string * option<System.Threading.CancellationToken> -> unit System.Threading.Tasks.Task
        /// Add a repository to a team
        OrgAddTeamRepository :
            int * string * string * option<System.Threading.CancellationToken> -> unit System.Threading.Tasks.Task
        /// search topics via keyword
        TopicSearch :
            string * int * int * option<System.Threading.CancellationToken>
                -> TopicResponse list System.Threading.Tasks.Task
        /// Get the authenticated user
        UserGetCurrent : option<System.Threading.CancellationToken> -> User System.Threading.Tasks.Task
        /// List the authenticated user's oauth2 applications
        UserGetOauth2Application :
            int * int * option<System.Threading.CancellationToken> -> OAuth2Application list System.Threading.Tasks.Task
        /// creates a new OAuth2 application
        UserCreateOAuth2Application :
            CreateOAuth2ApplicationOptions * option<System.Threading.CancellationToken>
                -> OAuth2Application System.Threading.Tasks.Task
        /// get an OAuth2 Application
        UserGetOAuth2Application :
            int * option<System.Threading.CancellationToken> -> OAuth2Application System.Threading.Tasks.Task
        /// delete an OAuth2 Application
        UserDeleteOAuth2Application :
            int * option<System.Threading.CancellationToken> -> unit System.Threading.Tasks.Task
        /// update an OAuth2 Application, this includes regenerating the client secret
        UserUpdateOAuth2Application :
            int * CreateOAuth2ApplicationOptions * option<System.Threading.CancellationToken>
                -> OAuth2Application System.Threading.Tasks.Task
        /// List the authenticated user's email addresses
        UserListEmails : option<System.Threading.CancellationToken> -> Email list System.Threading.Tasks.Task
        /// Add email addresses
        UserAddEmail :
            CreateEmailOption * option<System.Threading.CancellationToken> -> Email list System.Threading.Tasks.Task
        /// Delete email addresses
        UserDeleteEmail :
            DeleteEmailOption * option<System.Threading.CancellationToken> -> unit System.Threading.Tasks.Task
        /// List the authenticated user's followers
        UserCurrentListFollowers :
            int * int * option<System.Threading.CancellationToken> -> User list System.Threading.Tasks.Task
        /// List the users that the authenticated user is following
        UserCurrentListFollowing :
            int * int * option<System.Threading.CancellationToken> -> User list System.Threading.Tasks.Task
        /// Check whether a user is followed by the authenticated user
        UserCurrentCheckFollowing :
            string * option<System.Threading.CancellationToken> -> unit System.Threading.Tasks.Task
        /// Unfollow a user
        UserCurrentDeleteFollow :
            string * option<System.Threading.CancellationToken> -> unit System.Threading.Tasks.Task
        /// Follow a user
        UserCurrentPutFollow : string * option<System.Threading.CancellationToken> -> unit System.Threading.Tasks.Task
        /// Get a Token to verify
        GetVerificationToken : option<System.Threading.CancellationToken> -> string System.Threading.Tasks.Task
        /// Remove a GPG key
        UserCurrentDeleteGPGKey : int * option<System.Threading.CancellationToken> -> unit System.Threading.Tasks.Task
        /// List the authenticated user's public keys
        UserCurrentListKeys :
            string * int * int * option<System.Threading.CancellationToken>
                -> PublicKey list System.Threading.Tasks.Task
        /// Create a public key
        UserCurrentPostKey :
            CreateKeyOption * option<System.Threading.CancellationToken> -> PublicKey System.Threading.Tasks.Task
        /// Get a public key
        UserCurrentGetKey : int * option<System.Threading.CancellationToken> -> PublicKey System.Threading.Tasks.Task
        /// Delete a public key
        UserCurrentDeleteKey : int * option<System.Threading.CancellationToken> -> unit System.Threading.Tasks.Task
        /// List the current user's organizations
        OrgListCurrentUserOrgs :
            int * int * option<System.Threading.CancellationToken> -> Organization list System.Threading.Tasks.Task
        /// List the repos that the authenticated user owns
        UserCurrentListRepos :
            int * int * option<System.Threading.CancellationToken> -> Repository list System.Threading.Tasks.Task
        /// Create a repository
        CreateCurrentUserRepo :
            CreateRepoOption * option<System.Threading.CancellationToken> -> Repository System.Threading.Tasks.Task
        /// Get user settings
        GetUserSettings : option<System.Threading.CancellationToken> -> UserSettings list System.Threading.Tasks.Task
        /// Update user settings
        UpdateUserSettings :
            UserSettingsOptions * option<System.Threading.CancellationToken>
                -> UserSettings list System.Threading.Tasks.Task
        /// The repos that the authenticated user has starred
        UserCurrentListStarred :
            int * int * option<System.Threading.CancellationToken> -> Repository list System.Threading.Tasks.Task
        /// Whether the authenticated is starring the repo
        UserCurrentCheckStarring :
            string * string * option<System.Threading.CancellationToken> -> unit System.Threading.Tasks.Task
        /// Unstar the given repo
        UserCurrentDeleteStar :
            string * string * option<System.Threading.CancellationToken> -> unit System.Threading.Tasks.Task
        /// Star the given repo
        UserCurrentPutStar :
            string * string * option<System.Threading.CancellationToken> -> unit System.Threading.Tasks.Task
        /// Get list of all existing stopwatches
        UserGetStopWatches :
            int * int * option<System.Threading.CancellationToken> -> StopWatch list System.Threading.Tasks.Task
        /// List repositories watched by the authenticated user
        UserCurrentListSubscriptions :
            int * int * option<System.Threading.CancellationToken> -> Repository list System.Threading.Tasks.Task
        /// List all the teams a user belongs to
        UserListTeams : int * int * option<System.Threading.CancellationToken> -> Team list System.Threading.Tasks.Task
        /// List the current user's tracked times
        UserCurrentTrackedTimes :
            int * int * string * string * option<System.Threading.CancellationToken>
                -> TrackedTime list System.Threading.Tasks.Task
        /// Search for users
        UserSearch :
            string * int * int * int * option<System.Threading.CancellationToken> -> unit System.Threading.Tasks.Task
        /// Get a user
        UserGet : string * option<System.Threading.CancellationToken> -> User System.Threading.Tasks.Task
        /// List the given user's followers
        UserListFollowers :
            string * int * int * option<System.Threading.CancellationToken> -> User list System.Threading.Tasks.Task
        /// List the users that the given user is following
        UserListFollowing :
            string * int * int * option<System.Threading.CancellationToken> -> User list System.Threading.Tasks.Task
        /// Check if one user is following another user
        UserCheckFollowing :
            string * string * option<System.Threading.CancellationToken> -> unit System.Threading.Tasks.Task
        /// Get a user's heatmap
        UserGetHeatmapData :
            string * option<System.Threading.CancellationToken> -> UserHeatmapData list System.Threading.Tasks.Task
        /// List the given user's public keys
        UserListKeys :
            string * string * int * int * option<System.Threading.CancellationToken>
                -> PublicKey list System.Threading.Tasks.Task
        /// List a user's organizations
        OrgListUserOrgs :
            string * int * int * option<System.Threading.CancellationToken>
                -> Organization list System.Threading.Tasks.Task
        /// Get user permissions in organization
        OrgGetUserPermissions :
            string * string * option<System.Threading.CancellationToken>
                -> OrganizationPermissions System.Threading.Tasks.Task
        /// List the repos owned by the given user
        UserListRepos :
            string * int * int * option<System.Threading.CancellationToken>
                -> Repository list System.Threading.Tasks.Task
        /// The repos that the given user has starred
        UserListStarred :
            string * int * int * option<System.Threading.CancellationToken>
                -> Repository list System.Threading.Tasks.Task
        /// List the repositories watched by a user
        UserListSubscriptions :
            string * int * int * option<System.Threading.CancellationToken>
                -> Repository list System.Threading.Tasks.Task
        /// List the authenticated user's access tokens
        UserGetTokens :
            string * int * int * option<System.Threading.CancellationToken>
                -> AccessToken list System.Threading.Tasks.Task
        /// Create an access token
        UserCreateToken :
            string * CreateAccessTokenOption * option<System.Threading.CancellationToken>
                -> AccessToken System.Threading.Tasks.Task
        /// delete an access token
        UserDeleteAccessToken :
            string * string * option<System.Threading.CancellationToken> -> unit System.Threading.Tasks.Task
        /// Returns the version of the Gitea application
        GetVersion : option<System.Threading.CancellationToken> -> ServerVersion System.Threading.Tasks.Task
    }

    /// An implementation where every method throws.
    static member Empty : GiteaMock =
        {
            ActivitypubPerson =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: ActivitypubPerson"))
            ActivitypubPersonInbox =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: ActivitypubPersonInbox"))
            AdminCronList =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: AdminCronList"))
            AdminCronRun = (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: AdminCronRun"))
            AdminListHooks =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: AdminListHooks"))
            AdminCreateHook =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: AdminCreateHook"))
            AdminGetHook = (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: AdminGetHook"))
            AdminEditHook =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: AdminEditHook"))
            AdminGetAllOrgs =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: AdminGetAllOrgs"))
            AdminUnadoptedList =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: AdminUnadoptedList"))
            AdminAdoptRepository =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: AdminAdoptRepository"))
            AdminDeleteUnadoptedRepository =
                (fun _ ->
                    raise (
                        System.NotImplementedException "Unimplemented mock function: AdminDeleteUnadoptedRepository"
                    )
                )
            AdminGetAllUsers =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: AdminGetAllUsers"))
            AdminCreateUser =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: AdminCreateUser"))
            AdminDeleteUser =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: AdminDeleteUser"))
            AdminEditUser =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: AdminEditUser"))
            AdminCreatePublicKey =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: AdminCreatePublicKey"))
            AdminDeleteUserPublicKey =
                (fun _ ->
                    raise (System.NotImplementedException "Unimplemented mock function: AdminDeleteUserPublicKey")
                )
            AdminCreateOrg =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: AdminCreateOrg"))
            AdminCreateRepo =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: AdminCreateRepo"))
            AdminDeleteHook =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: AdminDeleteHook"))
            RenderMarkdown =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RenderMarkdown"))
            RenderMarkdownRaw =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RenderMarkdownRaw"))
            GetNodeInfo = (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: GetNodeInfo"))
            NotifyGetList =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: NotifyGetList"))
            NotifyReadList =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: NotifyReadList"))
            NotifyNewAvailable =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: NotifyNewAvailable"))
            NotifyGetThread =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: NotifyGetThread"))
            NotifyReadThread =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: NotifyReadThread"))
            CreateOrgRepoDeprecated =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: CreateOrgRepoDeprecated"))
            OrgGetAll = (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: OrgGetAll"))
            OrgCreate = (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: OrgCreate"))
            OrgGet = (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: OrgGet"))
            OrgDelete = (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: OrgDelete"))
            OrgEdit = (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: OrgEdit"))
            OrgListHooks = (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: OrgListHooks"))
            OrgCreateHook =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: OrgCreateHook"))
            OrgGetHook = (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: OrgGetHook"))
            OrgDeleteHook =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: OrgDeleteHook"))
            OrgEditHook = (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: OrgEditHook"))
            OrgListLabels =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: OrgListLabels"))
            OrgCreateLabel =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: OrgCreateLabel"))
            OrgGetLabel = (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: OrgGetLabel"))
            OrgDeleteLabel =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: OrgDeleteLabel"))
            OrgEditLabel = (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: OrgEditLabel"))
            OrgListMembers =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: OrgListMembers"))
            OrgIsMember = (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: OrgIsMember"))
            OrgDeleteMember =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: OrgDeleteMember"))
            OrgListPublicMembers =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: OrgListPublicMembers"))
            OrgIsPublicMember =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: OrgIsPublicMember"))
            OrgConcealMember =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: OrgConcealMember"))
            OrgPublicizeMember =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: OrgPublicizeMember"))
            OrgListRepos = (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: OrgListRepos"))
            CreateOrgRepo =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: CreateOrgRepo"))
            OrgListTeams = (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: OrgListTeams"))
            OrgCreateTeam =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: OrgCreateTeam"))
            TeamSearch = (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: TeamSearch"))
            ListPackages = (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: ListPackages"))
            GetPackage = (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: GetPackage"))
            DeletePackage =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: DeletePackage"))
            ListPackageFiles =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: ListPackageFiles"))
            IssueSearchIssues =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: IssueSearchIssues"))
            RepoMigrate = (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoMigrate"))
            RepoSearch = (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoSearch"))
            RepoGet = (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoGet"))
            RepoDelete = (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoDelete"))
            RepoEdit = (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoEdit"))
            RepoGetArchive =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoGetArchive"))
            RepoGetAssignees =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoGetAssignees"))
            RepoListBranchProtection =
                (fun _ ->
                    raise (System.NotImplementedException "Unimplemented mock function: RepoListBranchProtection")
                )
            RepoCreateBranchProtection =
                (fun _ ->
                    raise (System.NotImplementedException "Unimplemented mock function: RepoCreateBranchProtection")
                )
            RepoGetBranchProtection =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoGetBranchProtection"))
            RepoDeleteBranchProtection =
                (fun _ ->
                    raise (System.NotImplementedException "Unimplemented mock function: RepoDeleteBranchProtection")
                )
            RepoEditBranchProtection =
                (fun _ ->
                    raise (System.NotImplementedException "Unimplemented mock function: RepoEditBranchProtection")
                )
            RepoListBranches =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoListBranches"))
            RepoCreateBranch =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoCreateBranch"))
            RepoGetBranch =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoGetBranch"))
            RepoDeleteBranch =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoDeleteBranch"))
            RepoListCollaborators =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoListCollaborators"))
            RepoCheckCollaborator =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoCheckCollaborator"))
            RepoDeleteCollaborator =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoDeleteCollaborator"))
            RepoAddCollaborator =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoAddCollaborator"))
            RepoGetRepoPermissions =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoGetRepoPermissions"))
            RepoGetAllCommits =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoGetAllCommits"))
            RepoGetCombinedStatusByRef =
                (fun _ ->
                    raise (System.NotImplementedException "Unimplemented mock function: RepoGetCombinedStatusByRef")
                )
            RepoListStatusesByRef =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoListStatusesByRef"))
            RepoGetContentsList =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoGetContentsList"))
            RepoGetContents =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoGetContents"))
            RepoCreateFile =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoCreateFile"))
            RepoDeleteFile =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoDeleteFile"))
            RepoUpdateFile =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoUpdateFile"))
            RepoApplyDiffPatch =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoApplyDiffPatch"))
            RepoGetEditorConfig =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoGetEditorConfig"))
            ListForks = (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: ListForks"))
            CreateFork = (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: CreateFork"))
            GetBlob = (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: GetBlob"))
            RepoGetSingleCommit =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoGetSingleCommit"))
            RepoDownloadCommitDiffOrPatch =
                (fun _ ->
                    raise (System.NotImplementedException "Unimplemented mock function: RepoDownloadCommitDiffOrPatch")
                )
            RepoGetNote = (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoGetNote"))
            RepoListAllGitRefs =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoListAllGitRefs"))
            RepoListGitRefs =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoListGitRefs"))
            GetAnnotatedTag =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: GetAnnotatedTag"))
            GetTree = (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: GetTree"))
            RepoListHooks =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoListHooks"))
            RepoCreateHook =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoCreateHook"))
            RepoListGitHooks =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoListGitHooks"))
            RepoGetGitHook =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoGetGitHook"))
            RepoDeleteGitHook =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoDeleteGitHook"))
            RepoEditGitHook =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoEditGitHook"))
            RepoGetHook = (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoGetHook"))
            RepoDeleteHook =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoDeleteHook"))
            RepoEditHook = (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoEditHook"))
            RepoTestHook = (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoTestHook"))
            RepoGetIssueTemplates =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoGetIssueTemplates"))
            IssueListIssues =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: IssueListIssues"))
            IssueCreateIssue =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: IssueCreateIssue"))
            IssueGetRepoComments =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: IssueGetRepoComments"))
            IssueDeleteComment =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: IssueDeleteComment"))
            IssueListIssueCommentAttachments =
                (fun _ ->
                    raise (
                        System.NotImplementedException "Unimplemented mock function: IssueListIssueCommentAttachments"
                    )
                )
            IssueGetIssueCommentAttachment =
                (fun _ ->
                    raise (
                        System.NotImplementedException "Unimplemented mock function: IssueGetIssueCommentAttachment"
                    )
                )
            IssueDeleteIssueCommentAttachment =
                (fun _ ->
                    raise (
                        System.NotImplementedException "Unimplemented mock function: IssueDeleteIssueCommentAttachment"
                    )
                )
            IssueEditIssueCommentAttachment =
                (fun _ ->
                    raise (
                        System.NotImplementedException "Unimplemented mock function: IssueEditIssueCommentAttachment"
                    )
                )
            IssueGetCommentReactions =
                (fun _ ->
                    raise (System.NotImplementedException "Unimplemented mock function: IssueGetCommentReactions")
                )
            IssueDeleteCommentReaction =
                (fun _ ->
                    raise (System.NotImplementedException "Unimplemented mock function: IssueDeleteCommentReaction")
                )
            IssueGetIssue =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: IssueGetIssue"))
            IssueDelete = (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: IssueDelete"))
            IssueEditIssue =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: IssueEditIssue"))
            IssueListIssueAttachments =
                (fun _ ->
                    raise (System.NotImplementedException "Unimplemented mock function: IssueListIssueAttachments")
                )
            IssueGetIssueAttachment =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: IssueGetIssueAttachment"))
            IssueDeleteIssueAttachment =
                (fun _ ->
                    raise (System.NotImplementedException "Unimplemented mock function: IssueDeleteIssueAttachment")
                )
            IssueEditIssueAttachment =
                (fun _ ->
                    raise (System.NotImplementedException "Unimplemented mock function: IssueEditIssueAttachment")
                )
            IssueGetComments =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: IssueGetComments"))
            IssueCreateComment =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: IssueCreateComment"))
            IssueDeleteCommentDeprecated =
                (fun _ ->
                    raise (System.NotImplementedException "Unimplemented mock function: IssueDeleteCommentDeprecated")
                )
            IssueEditIssueDeadline =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: IssueEditIssueDeadline"))
            IssueGetLabels =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: IssueGetLabels"))
            IssueAddLabel =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: IssueAddLabel"))
            IssueClearLabels =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: IssueClearLabels"))
            IssueReplaceLabels =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: IssueReplaceLabels"))
            IssueRemoveLabel =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: IssueRemoveLabel"))
            IssueGetIssueReactions =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: IssueGetIssueReactions"))
            IssueDeleteIssueReaction =
                (fun _ ->
                    raise (System.NotImplementedException "Unimplemented mock function: IssueDeleteIssueReaction")
                )
            IssueDeleteStopWatch =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: IssueDeleteStopWatch"))
            IssueStartStopWatch =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: IssueStartStopWatch"))
            IssueStopStopWatch =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: IssueStopStopWatch"))
            IssueSubscriptions =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: IssueSubscriptions"))
            IssueCheckSubscription =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: IssueCheckSubscription"))
            IssueGetCommentsAndTimeline =
                (fun _ ->
                    raise (System.NotImplementedException "Unimplemented mock function: IssueGetCommentsAndTimeline")
                )
            IssueTrackedTimes =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: IssueTrackedTimes"))
            IssueAddTime = (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: IssueAddTime"))
            IssueResetTime =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: IssueResetTime"))
            IssueDeleteTime =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: IssueDeleteTime"))
            RepoListKeys = (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoListKeys"))
            RepoCreateKey =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoCreateKey"))
            RepoGetKey = (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoGetKey"))
            RepoDeleteKey =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoDeleteKey"))
            IssueListLabels =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: IssueListLabels"))
            IssueCreateLabel =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: IssueCreateLabel"))
            IssueGetLabel =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: IssueGetLabel"))
            IssueDeleteLabel =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: IssueDeleteLabel"))
            IssueEditLabel =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: IssueEditLabel"))
            RepoGetLanguages =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoGetLanguages"))
            RepoGetRawFileOrLFS =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoGetRawFileOrLFS"))
            IssueGetMilestonesList =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: IssueGetMilestonesList"))
            IssueCreateMilestone =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: IssueCreateMilestone"))
            IssueGetMilestone =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: IssueGetMilestone"))
            IssueDeleteMilestone =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: IssueDeleteMilestone"))
            IssueEditMilestone =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: IssueEditMilestone"))
            RepoMirrorSync =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoMirrorSync"))
            NotifyGetRepoList =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: NotifyGetRepoList"))
            NotifyReadRepoList =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: NotifyReadRepoList"))
            RepoListPullRequests =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoListPullRequests"))
            RepoCreatePullRequest =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoCreatePullRequest"))
            RepoGetPullRequest =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoGetPullRequest"))
            RepoEditPullRequest =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoEditPullRequest"))
            RepoDownloadPullDiffOrPatch =
                (fun _ ->
                    raise (System.NotImplementedException "Unimplemented mock function: RepoDownloadPullDiffOrPatch")
                )
            RepoGetPullRequestCommits =
                (fun _ ->
                    raise (System.NotImplementedException "Unimplemented mock function: RepoGetPullRequestCommits")
                )
            RepoGetPullRequestFiles =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoGetPullRequestFiles"))
            RepoPullRequestIsMerged =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoPullRequestIsMerged"))
            RepoMergePullRequest =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoMergePullRequest"))
            RepoCancelScheduledAutoMerge =
                (fun _ ->
                    raise (System.NotImplementedException "Unimplemented mock function: RepoCancelScheduledAutoMerge")
                )
            RepoCreatePullReviewRequests =
                (fun _ ->
                    raise (System.NotImplementedException "Unimplemented mock function: RepoCreatePullReviewRequests")
                )
            RepoDeletePullReviewRequests =
                (fun _ ->
                    raise (System.NotImplementedException "Unimplemented mock function: RepoDeletePullReviewRequests")
                )
            RepoListPullReviews =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoListPullReviews"))
            RepoCreatePullReview =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoCreatePullReview"))
            RepoGetPullReview =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoGetPullReview"))
            RepoSubmitPullReview =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoSubmitPullReview"))
            RepoDeletePullReview =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoDeletePullReview"))
            RepoGetPullReviewComments =
                (fun _ ->
                    raise (System.NotImplementedException "Unimplemented mock function: RepoGetPullReviewComments")
                )
            RepoDismissPullReview =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoDismissPullReview"))
            RepoUnDismissPullReview =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoUnDismissPullReview"))
            RepoUpdatePullRequest =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoUpdatePullRequest"))
            RepoListPushMirrors =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoListPushMirrors"))
            RepoAddPushMirror =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoAddPushMirror"))
            RepoPushMirrorSync =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoPushMirrorSync"))
            RepoGetPushMirrorByRemoteName =
                (fun _ ->
                    raise (System.NotImplementedException "Unimplemented mock function: RepoGetPushMirrorByRemoteName")
                )
            RepoDeletePushMirror =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoDeletePushMirror"))
            RepoGetRawFile =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoGetRawFile"))
            RepoListReleases =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoListReleases"))
            RepoCreateRelease =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoCreateRelease"))
            RepoGetLatestRelease =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoGetLatestRelease"))
            RepoGetReleaseByTag =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoGetReleaseByTag"))
            RepoDeleteReleaseByTag =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoDeleteReleaseByTag"))
            RepoGetRelease =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoGetRelease"))
            RepoDeleteRelease =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoDeleteRelease"))
            RepoEditRelease =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoEditRelease"))
            RepoListReleaseAttachments =
                (fun _ ->
                    raise (System.NotImplementedException "Unimplemented mock function: RepoListReleaseAttachments")
                )
            RepoGetReleaseAttachment =
                (fun _ ->
                    raise (System.NotImplementedException "Unimplemented mock function: RepoGetReleaseAttachment")
                )
            RepoDeleteReleaseAttachment =
                (fun _ ->
                    raise (System.NotImplementedException "Unimplemented mock function: RepoDeleteReleaseAttachment")
                )
            RepoEditReleaseAttachment =
                (fun _ ->
                    raise (System.NotImplementedException "Unimplemented mock function: RepoEditReleaseAttachment")
                )
            RepoGetReviewers =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoGetReviewers"))
            RepoSigningKey =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoSigningKey"))
            RepoListStargazers =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoListStargazers"))
            RepoListStatuses =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoListStatuses"))
            RepoCreateStatus =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoCreateStatus"))
            RepoListSubscribers =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoListSubscribers"))
            UserCurrentCheckSubscription =
                (fun _ ->
                    raise (System.NotImplementedException "Unimplemented mock function: UserCurrentCheckSubscription")
                )
            UserCurrentDeleteSubscription =
                (fun _ ->
                    raise (System.NotImplementedException "Unimplemented mock function: UserCurrentDeleteSubscription")
                )
            UserCurrentPutSubscription =
                (fun _ ->
                    raise (System.NotImplementedException "Unimplemented mock function: UserCurrentPutSubscription")
                )
            RepoListTags = (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoListTags"))
            RepoCreateTag =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoCreateTag"))
            RepoGetTag = (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoGetTag"))
            RepoDeleteTag =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoDeleteTag"))
            RepoListTeams =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoListTeams"))
            RepoCheckTeam =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoCheckTeam"))
            RepoDeleteTeam =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoDeleteTeam"))
            RepoAddTeam = (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoAddTeam"))
            RepoTrackedTimes =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoTrackedTimes"))
            UserTrackedTimes =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: UserTrackedTimes"))
            RepoListTopics =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoListTopics"))
            RepoUpdateTopics =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoUpdateTopics"))
            RepoDeleteTopic =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoDeleteTopic"))
            RepoAddTopic = (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoAddTopic"))
            RepoTransfer = (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoTransfer"))
            AcceptRepoTransfer =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: AcceptRepoTransfer"))
            RejectRepoTransfer =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RejectRepoTransfer"))
            RepoCreateWikiPage =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoCreateWikiPage"))
            RepoGetWikiPage =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoGetWikiPage"))
            RepoDeleteWikiPage =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoDeleteWikiPage"))
            RepoEditWikiPage =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoEditWikiPage"))
            RepoGetWikiPages =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoGetWikiPages"))
            RepoGetWikiPageRevisions =
                (fun _ ->
                    raise (System.NotImplementedException "Unimplemented mock function: RepoGetWikiPageRevisions")
                )
            GenerateRepo = (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: GenerateRepo"))
            RepoGetByID = (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: RepoGetByID"))
            GetGeneralAPISettings =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: GetGeneralAPISettings"))
            GetGeneralAttachmentSettings =
                (fun _ ->
                    raise (System.NotImplementedException "Unimplemented mock function: GetGeneralAttachmentSettings")
                )
            GetGeneralRepositorySettings =
                (fun _ ->
                    raise (System.NotImplementedException "Unimplemented mock function: GetGeneralRepositorySettings")
                )
            GetGeneralUISettings =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: GetGeneralUISettings"))
            GetSigningKey =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: GetSigningKey"))
            OrgGetTeam = (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: OrgGetTeam"))
            OrgDeleteTeam =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: OrgDeleteTeam"))
            OrgEditTeam = (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: OrgEditTeam"))
            OrgListTeamMembers =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: OrgListTeamMembers"))
            OrgListTeamMember =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: OrgListTeamMember"))
            OrgRemoveTeamMember =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: OrgRemoveTeamMember"))
            OrgAddTeamMember =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: OrgAddTeamMember"))
            OrgListTeamRepos =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: OrgListTeamRepos"))
            OrgListTeamRepo =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: OrgListTeamRepo"))
            OrgRemoveTeamRepository =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: OrgRemoveTeamRepository"))
            OrgAddTeamRepository =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: OrgAddTeamRepository"))
            TopicSearch = (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: TopicSearch"))
            UserGetCurrent =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: UserGetCurrent"))
            UserGetOauth2Application =
                (fun _ ->
                    raise (System.NotImplementedException "Unimplemented mock function: UserGetOauth2Application")
                )
            UserCreateOAuth2Application =
                (fun _ ->
                    raise (System.NotImplementedException "Unimplemented mock function: UserCreateOAuth2Application")
                )
            UserGetOAuth2Application =
                (fun _ ->
                    raise (System.NotImplementedException "Unimplemented mock function: UserGetOAuth2Application")
                )
            UserDeleteOAuth2Application =
                (fun _ ->
                    raise (System.NotImplementedException "Unimplemented mock function: UserDeleteOAuth2Application")
                )
            UserUpdateOAuth2Application =
                (fun _ ->
                    raise (System.NotImplementedException "Unimplemented mock function: UserUpdateOAuth2Application")
                )
            UserListEmails =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: UserListEmails"))
            UserAddEmail = (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: UserAddEmail"))
            UserDeleteEmail =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: UserDeleteEmail"))
            UserCurrentListFollowers =
                (fun _ ->
                    raise (System.NotImplementedException "Unimplemented mock function: UserCurrentListFollowers")
                )
            UserCurrentListFollowing =
                (fun _ ->
                    raise (System.NotImplementedException "Unimplemented mock function: UserCurrentListFollowing")
                )
            UserCurrentCheckFollowing =
                (fun _ ->
                    raise (System.NotImplementedException "Unimplemented mock function: UserCurrentCheckFollowing")
                )
            UserCurrentDeleteFollow =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: UserCurrentDeleteFollow"))
            UserCurrentPutFollow =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: UserCurrentPutFollow"))
            GetVerificationToken =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: GetVerificationToken"))
            UserCurrentDeleteGPGKey =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: UserCurrentDeleteGPGKey"))
            UserCurrentListKeys =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: UserCurrentListKeys"))
            UserCurrentPostKey =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: UserCurrentPostKey"))
            UserCurrentGetKey =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: UserCurrentGetKey"))
            UserCurrentDeleteKey =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: UserCurrentDeleteKey"))
            OrgListCurrentUserOrgs =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: OrgListCurrentUserOrgs"))
            UserCurrentListRepos =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: UserCurrentListRepos"))
            CreateCurrentUserRepo =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: CreateCurrentUserRepo"))
            GetUserSettings =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: GetUserSettings"))
            UpdateUserSettings =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: UpdateUserSettings"))
            UserCurrentListStarred =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: UserCurrentListStarred"))
            UserCurrentCheckStarring =
                (fun _ ->
                    raise (System.NotImplementedException "Unimplemented mock function: UserCurrentCheckStarring")
                )
            UserCurrentDeleteStar =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: UserCurrentDeleteStar"))
            UserCurrentPutStar =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: UserCurrentPutStar"))
            UserGetStopWatches =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: UserGetStopWatches"))
            UserCurrentListSubscriptions =
                (fun _ ->
                    raise (System.NotImplementedException "Unimplemented mock function: UserCurrentListSubscriptions")
                )
            UserListTeams =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: UserListTeams"))
            UserCurrentTrackedTimes =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: UserCurrentTrackedTimes"))
            UserSearch = (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: UserSearch"))
            UserGet = (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: UserGet"))
            UserListFollowers =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: UserListFollowers"))
            UserListFollowing =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: UserListFollowing"))
            UserCheckFollowing =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: UserCheckFollowing"))
            UserGetHeatmapData =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: UserGetHeatmapData"))
            UserListKeys = (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: UserListKeys"))
            OrgListUserOrgs =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: OrgListUserOrgs"))
            OrgGetUserPermissions =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: OrgGetUserPermissions"))
            UserListRepos =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: UserListRepos"))
            UserListStarred =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: UserListStarred"))
            UserListSubscriptions =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: UserListSubscriptions"))
            UserGetTokens =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: UserGetTokens"))
            UserCreateToken =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: UserCreateToken"))
            UserDeleteAccessToken =
                (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: UserDeleteAccessToken"))
            GetVersion = (fun _ -> raise (System.NotImplementedException "Unimplemented mock function: GetVersion"))
        }

    interface IGitea with
        member this.ActivitypubPerson (arg_0_0, arg_0_1) =
            this.ActivitypubPerson (arg_0_0, arg_0_1)

        member this.ActivitypubPersonInbox (arg_0_0, arg_0_1) =
            this.ActivitypubPersonInbox (arg_0_0, arg_0_1)

        member this.AdminCronList (arg_0_0, arg_0_1, arg_0_2) =
            this.AdminCronList (arg_0_0, arg_0_1, arg_0_2)

        member this.AdminCronRun (arg_0_0, arg_0_1) = this.AdminCronRun (arg_0_0, arg_0_1)

        member this.AdminListHooks (arg_0_0, arg_0_1, arg_0_2) =
            this.AdminListHooks (arg_0_0, arg_0_1, arg_0_2)

        member this.AdminCreateHook (arg_0_0, arg_0_1) = this.AdminCreateHook (arg_0_0, arg_0_1)
        member this.AdminGetHook (arg_0_0, arg_0_1) = this.AdminGetHook (arg_0_0, arg_0_1)

        member this.AdminEditHook (arg_0_0, arg_0_1, arg_0_2) =
            this.AdminEditHook (arg_0_0, arg_0_1, arg_0_2)

        member this.AdminGetAllOrgs (arg_0_0, arg_0_1, arg_0_2) =
            this.AdminGetAllOrgs (arg_0_0, arg_0_1, arg_0_2)

        member this.AdminUnadoptedList (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.AdminUnadoptedList (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.AdminAdoptRepository (arg_0_0, arg_0_1, arg_0_2) =
            this.AdminAdoptRepository (arg_0_0, arg_0_1, arg_0_2)

        member this.AdminDeleteUnadoptedRepository (arg_0_0, arg_0_1, arg_0_2) =
            this.AdminDeleteUnadoptedRepository (arg_0_0, arg_0_1, arg_0_2)

        member this.AdminGetAllUsers (arg_0_0, arg_0_1, arg_0_2) =
            this.AdminGetAllUsers (arg_0_0, arg_0_1, arg_0_2)

        member this.AdminCreateUser (arg_0_0, arg_0_1) = this.AdminCreateUser (arg_0_0, arg_0_1)

        member this.AdminDeleteUser (arg_0_0, arg_0_1, arg_0_2) =
            this.AdminDeleteUser (arg_0_0, arg_0_1, arg_0_2)

        member this.AdminEditUser (arg_0_0, arg_0_1, arg_0_2) =
            this.AdminEditUser (arg_0_0, arg_0_1, arg_0_2)

        member this.AdminCreatePublicKey (arg_0_0, arg_0_1, arg_0_2) =
            this.AdminCreatePublicKey (arg_0_0, arg_0_1, arg_0_2)

        member this.AdminDeleteUserPublicKey (arg_0_0, arg_0_1, arg_0_2) =
            this.AdminDeleteUserPublicKey (arg_0_0, arg_0_1, arg_0_2)

        member this.AdminCreateOrg (arg_0_0, arg_0_1, arg_0_2) =
            this.AdminCreateOrg (arg_0_0, arg_0_1, arg_0_2)

        member this.AdminCreateRepo (arg_0_0, arg_0_1, arg_0_2) =
            this.AdminCreateRepo (arg_0_0, arg_0_1, arg_0_2)

        member this.AdminDeleteHook (arg_0_0, arg_0_1) = this.AdminDeleteHook (arg_0_0, arg_0_1)
        member this.RenderMarkdown (arg_0_0, arg_0_1) = this.RenderMarkdown (arg_0_0, arg_0_1)

        member this.RenderMarkdownRaw (arg_0_0, arg_0_1) =
            this.RenderMarkdownRaw (arg_0_0, arg_0_1)

        member this.GetNodeInfo arg_0_0 = this.GetNodeInfo (arg_0_0)

        member this.NotifyGetList (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4, arg_0_5, arg_0_6, arg_0_7) =
            this.NotifyGetList (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4, arg_0_5, arg_0_6, arg_0_7)

        member this.NotifyReadList (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4) =
            this.NotifyReadList (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4)

        member this.NotifyNewAvailable arg_0_0 = this.NotifyNewAvailable (arg_0_0)
        member this.NotifyGetThread (arg_0_0, arg_0_1) = this.NotifyGetThread (arg_0_0, arg_0_1)

        member this.NotifyReadThread (arg_0_0, arg_0_1, arg_0_2) =
            this.NotifyReadThread (arg_0_0, arg_0_1, arg_0_2)

        member this.CreateOrgRepoDeprecated (arg_0_0, arg_0_1, arg_0_2) =
            this.CreateOrgRepoDeprecated (arg_0_0, arg_0_1, arg_0_2)

        member this.OrgGetAll (arg_0_0, arg_0_1, arg_0_2) =
            this.OrgGetAll (arg_0_0, arg_0_1, arg_0_2)

        member this.OrgCreate (arg_0_0, arg_0_1) = this.OrgCreate (arg_0_0, arg_0_1)
        member this.OrgGet (arg_0_0, arg_0_1) = this.OrgGet (arg_0_0, arg_0_1)
        member this.OrgDelete (arg_0_0, arg_0_1) = this.OrgDelete (arg_0_0, arg_0_1)

        member this.OrgEdit (arg_0_0, arg_0_1, arg_0_2) =
            this.OrgEdit (arg_0_0, arg_0_1, arg_0_2)

        member this.OrgListHooks (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.OrgListHooks (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.OrgCreateHook (arg_0_0, arg_0_1, arg_0_2) =
            this.OrgCreateHook (arg_0_0, arg_0_1, arg_0_2)

        member this.OrgGetHook (arg_0_0, arg_0_1, arg_0_2) =
            this.OrgGetHook (arg_0_0, arg_0_1, arg_0_2)

        member this.OrgDeleteHook (arg_0_0, arg_0_1, arg_0_2) =
            this.OrgDeleteHook (arg_0_0, arg_0_1, arg_0_2)

        member this.OrgEditHook (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.OrgEditHook (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.OrgListLabels (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.OrgListLabels (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.OrgCreateLabel (arg_0_0, arg_0_1, arg_0_2) =
            this.OrgCreateLabel (arg_0_0, arg_0_1, arg_0_2)

        member this.OrgGetLabel (arg_0_0, arg_0_1, arg_0_2) =
            this.OrgGetLabel (arg_0_0, arg_0_1, arg_0_2)

        member this.OrgDeleteLabel (arg_0_0, arg_0_1, arg_0_2) =
            this.OrgDeleteLabel (arg_0_0, arg_0_1, arg_0_2)

        member this.OrgEditLabel (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.OrgEditLabel (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.OrgListMembers (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.OrgListMembers (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.OrgIsMember (arg_0_0, arg_0_1, arg_0_2) =
            this.OrgIsMember (arg_0_0, arg_0_1, arg_0_2)

        member this.OrgDeleteMember (arg_0_0, arg_0_1, arg_0_2) =
            this.OrgDeleteMember (arg_0_0, arg_0_1, arg_0_2)

        member this.OrgListPublicMembers (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.OrgListPublicMembers (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.OrgIsPublicMember (arg_0_0, arg_0_1, arg_0_2) =
            this.OrgIsPublicMember (arg_0_0, arg_0_1, arg_0_2)

        member this.OrgConcealMember (arg_0_0, arg_0_1, arg_0_2) =
            this.OrgConcealMember (arg_0_0, arg_0_1, arg_0_2)

        member this.OrgPublicizeMember (arg_0_0, arg_0_1, arg_0_2) =
            this.OrgPublicizeMember (arg_0_0, arg_0_1, arg_0_2)

        member this.OrgListRepos (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.OrgListRepos (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.CreateOrgRepo (arg_0_0, arg_0_1, arg_0_2) =
            this.CreateOrgRepo (arg_0_0, arg_0_1, arg_0_2)

        member this.OrgListTeams (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.OrgListTeams (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.OrgCreateTeam (arg_0_0, arg_0_1, arg_0_2) =
            this.OrgCreateTeam (arg_0_0, arg_0_1, arg_0_2)

        member this.TeamSearch (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4, arg_0_5) =
            this.TeamSearch (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4, arg_0_5)

        member this.ListPackages (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4, arg_0_5) =
            this.ListPackages (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4, arg_0_5)

        member this.GetPackage (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4) =
            this.GetPackage (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4)

        member this.DeletePackage (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4) =
            this.DeletePackage (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4)

        member this.ListPackageFiles (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4) =
            this.ListPackageFiles (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4)

        member this.IssueSearchIssues
            (
                arg_0_0,
                arg_0_1,
                arg_0_2,
                arg_0_3,
                arg_0_4,
                arg_0_5,
                arg_0_6,
                arg_0_7,
                arg_0_8,
                arg_0_9,
                arg_0_10,
                arg_0_11,
                arg_0_12,
                arg_0_13,
                arg_0_14,
                arg_0_15,
                arg_0_16
            )
            =
            this.IssueSearchIssues (
                arg_0_0,
                arg_0_1,
                arg_0_2,
                arg_0_3,
                arg_0_4,
                arg_0_5,
                arg_0_6,
                arg_0_7,
                arg_0_8,
                arg_0_9,
                arg_0_10,
                arg_0_11,
                arg_0_12,
                arg_0_13,
                arg_0_14,
                arg_0_15,
                arg_0_16
            )

        member this.RepoMigrate (arg_0_0, arg_0_1) = this.RepoMigrate (arg_0_0, arg_0_1)

        member this.RepoSearch
            (
                arg_0_0,
                arg_0_1,
                arg_0_2,
                arg_0_3,
                arg_0_4,
                arg_0_5,
                arg_0_6,
                arg_0_7,
                arg_0_8,
                arg_0_9,
                arg_0_10,
                arg_0_11,
                arg_0_12,
                arg_0_13,
                arg_0_14,
                arg_0_15,
                arg_0_16,
                arg_0_17
            )
            =
            this.RepoSearch (
                arg_0_0,
                arg_0_1,
                arg_0_2,
                arg_0_3,
                arg_0_4,
                arg_0_5,
                arg_0_6,
                arg_0_7,
                arg_0_8,
                arg_0_9,
                arg_0_10,
                arg_0_11,
                arg_0_12,
                arg_0_13,
                arg_0_14,
                arg_0_15,
                arg_0_16,
                arg_0_17
            )

        member this.RepoGet (arg_0_0, arg_0_1, arg_0_2) =
            this.RepoGet (arg_0_0, arg_0_1, arg_0_2)

        member this.RepoDelete (arg_0_0, arg_0_1, arg_0_2) =
            this.RepoDelete (arg_0_0, arg_0_1, arg_0_2)

        member this.RepoEdit (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.RepoEdit (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.RepoGetArchive (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.RepoGetArchive (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.RepoGetAssignees (arg_0_0, arg_0_1, arg_0_2) =
            this.RepoGetAssignees (arg_0_0, arg_0_1, arg_0_2)

        member this.RepoListBranchProtection (arg_0_0, arg_0_1, arg_0_2) =
            this.RepoListBranchProtection (arg_0_0, arg_0_1, arg_0_2)

        member this.RepoCreateBranchProtection (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.RepoCreateBranchProtection (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.RepoGetBranchProtection (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.RepoGetBranchProtection (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.RepoDeleteBranchProtection (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.RepoDeleteBranchProtection (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.RepoEditBranchProtection (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4) =
            this.RepoEditBranchProtection (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4)

        member this.RepoListBranches (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4) =
            this.RepoListBranches (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4)

        member this.RepoCreateBranch (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.RepoCreateBranch (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.RepoGetBranch (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.RepoGetBranch (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.RepoDeleteBranch (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.RepoDeleteBranch (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.RepoListCollaborators (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4) =
            this.RepoListCollaborators (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4)

        member this.RepoCheckCollaborator (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.RepoCheckCollaborator (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.RepoDeleteCollaborator (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.RepoDeleteCollaborator (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.RepoAddCollaborator (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4) =
            this.RepoAddCollaborator (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4)

        member this.RepoGetRepoPermissions (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.RepoGetRepoPermissions (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.RepoGetAllCommits (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4, arg_0_5, arg_0_6, arg_0_7) =
            this.RepoGetAllCommits (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4, arg_0_5, arg_0_6, arg_0_7)

        member this.RepoGetCombinedStatusByRef (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4, arg_0_5) =
            this.RepoGetCombinedStatusByRef (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4, arg_0_5)

        member this.RepoListStatusesByRef (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4, arg_0_5, arg_0_6, arg_0_7) =
            this.RepoListStatusesByRef (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4, arg_0_5, arg_0_6, arg_0_7)

        member this.RepoGetContentsList (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.RepoGetContentsList (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.RepoGetContents (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4) =
            this.RepoGetContents (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4)

        member this.RepoCreateFile (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4) =
            this.RepoCreateFile (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4)

        member this.RepoDeleteFile (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4) =
            this.RepoDeleteFile (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4)

        member this.RepoUpdateFile (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4) =
            this.RepoUpdateFile (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4)

        member this.RepoApplyDiffPatch (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.RepoApplyDiffPatch (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.RepoGetEditorConfig (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4) =
            this.RepoGetEditorConfig (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4)

        member this.ListForks (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4) =
            this.ListForks (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4)

        member this.CreateFork (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.CreateFork (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.GetBlob (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.GetBlob (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.RepoGetSingleCommit (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.RepoGetSingleCommit (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.RepoDownloadCommitDiffOrPatch (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4) =
            this.RepoDownloadCommitDiffOrPatch (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4)

        member this.RepoGetNote (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.RepoGetNote (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.RepoListAllGitRefs (arg_0_0, arg_0_1, arg_0_2) =
            this.RepoListAllGitRefs (arg_0_0, arg_0_1, arg_0_2)

        member this.RepoListGitRefs (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.RepoListGitRefs (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.GetAnnotatedTag (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.GetAnnotatedTag (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.GetTree (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4, arg_0_5, arg_0_6) =
            this.GetTree (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4, arg_0_5, arg_0_6)

        member this.RepoListHooks (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4) =
            this.RepoListHooks (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4)

        member this.RepoCreateHook (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.RepoCreateHook (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.RepoListGitHooks (arg_0_0, arg_0_1, arg_0_2) =
            this.RepoListGitHooks (arg_0_0, arg_0_1, arg_0_2)

        member this.RepoGetGitHook (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.RepoGetGitHook (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.RepoDeleteGitHook (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.RepoDeleteGitHook (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.RepoEditGitHook (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4) =
            this.RepoEditGitHook (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4)

        member this.RepoGetHook (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.RepoGetHook (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.RepoDeleteHook (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.RepoDeleteHook (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.RepoEditHook (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4) =
            this.RepoEditHook (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4)

        member this.RepoTestHook (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4) =
            this.RepoTestHook (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4)

        member this.RepoGetIssueTemplates (arg_0_0, arg_0_1, arg_0_2) =
            this.RepoGetIssueTemplates (arg_0_0, arg_0_1, arg_0_2)

        member this.IssueListIssues
            (
                arg_0_0,
                arg_0_1,
                arg_0_2,
                arg_0_3,
                arg_0_4,
                arg_0_5,
                arg_0_6,
                arg_0_7,
                arg_0_8,
                arg_0_9,
                arg_0_10,
                arg_0_11,
                arg_0_12,
                arg_0_13,
                arg_0_14
            )
            =
            this.IssueListIssues (
                arg_0_0,
                arg_0_1,
                arg_0_2,
                arg_0_3,
                arg_0_4,
                arg_0_5,
                arg_0_6,
                arg_0_7,
                arg_0_8,
                arg_0_9,
                arg_0_10,
                arg_0_11,
                arg_0_12,
                arg_0_13,
                arg_0_14
            )

        member this.IssueCreateIssue (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.IssueCreateIssue (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.IssueGetRepoComments (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4, arg_0_5, arg_0_6) =
            this.IssueGetRepoComments (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4, arg_0_5, arg_0_6)

        member this.IssueDeleteComment (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.IssueDeleteComment (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.IssueListIssueCommentAttachments (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.IssueListIssueCommentAttachments (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.IssueGetIssueCommentAttachment (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4) =
            this.IssueGetIssueCommentAttachment (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4)

        member this.IssueDeleteIssueCommentAttachment (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4) =
            this.IssueDeleteIssueCommentAttachment (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4)

        member this.IssueEditIssueCommentAttachment (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4, arg_0_5) =
            this.IssueEditIssueCommentAttachment (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4, arg_0_5)

        member this.IssueGetCommentReactions (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.IssueGetCommentReactions (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.IssueDeleteCommentReaction (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4) =
            this.IssueDeleteCommentReaction (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4)

        member this.IssueGetIssue (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.IssueGetIssue (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.IssueDelete (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.IssueDelete (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.IssueEditIssue (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4) =
            this.IssueEditIssue (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4)

        member this.IssueListIssueAttachments (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.IssueListIssueAttachments (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.IssueGetIssueAttachment (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4) =
            this.IssueGetIssueAttachment (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4)

        member this.IssueDeleteIssueAttachment (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4) =
            this.IssueDeleteIssueAttachment (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4)

        member this.IssueEditIssueAttachment (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4, arg_0_5) =
            this.IssueEditIssueAttachment (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4, arg_0_5)

        member this.IssueGetComments (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4, arg_0_5) =
            this.IssueGetComments (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4, arg_0_5)

        member this.IssueCreateComment (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4) =
            this.IssueCreateComment (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4)

        member this.IssueDeleteCommentDeprecated (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4) =
            this.IssueDeleteCommentDeprecated (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4)

        member this.IssueEditIssueDeadline (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4) =
            this.IssueEditIssueDeadline (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4)

        member this.IssueGetLabels (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.IssueGetLabels (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.IssueAddLabel (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4) =
            this.IssueAddLabel (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4)

        member this.IssueClearLabels (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.IssueClearLabels (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.IssueReplaceLabels (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4) =
            this.IssueReplaceLabels (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4)

        member this.IssueRemoveLabel (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4) =
            this.IssueRemoveLabel (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4)

        member this.IssueGetIssueReactions (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4, arg_0_5) =
            this.IssueGetIssueReactions (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4, arg_0_5)

        member this.IssueDeleteIssueReaction (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4) =
            this.IssueDeleteIssueReaction (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4)

        member this.IssueDeleteStopWatch (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.IssueDeleteStopWatch (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.IssueStartStopWatch (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.IssueStartStopWatch (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.IssueStopStopWatch (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.IssueStopStopWatch (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.IssueSubscriptions (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4, arg_0_5) =
            this.IssueSubscriptions (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4, arg_0_5)

        member this.IssueCheckSubscription (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.IssueCheckSubscription (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.IssueGetCommentsAndTimeline
            (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4, arg_0_5, arg_0_6, arg_0_7)
            =
            this.IssueGetCommentsAndTimeline (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4, arg_0_5, arg_0_6, arg_0_7)

        member this.IssueTrackedTimes
            (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4, arg_0_5, arg_0_6, arg_0_7, arg_0_8)
            =
            this.IssueTrackedTimes (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4, arg_0_5, arg_0_6, arg_0_7, arg_0_8)

        member this.IssueAddTime (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4) =
            this.IssueAddTime (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4)

        member this.IssueResetTime (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.IssueResetTime (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.IssueDeleteTime (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4) =
            this.IssueDeleteTime (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4)

        member this.RepoListKeys (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4, arg_0_5, arg_0_6) =
            this.RepoListKeys (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4, arg_0_5, arg_0_6)

        member this.RepoCreateKey (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.RepoCreateKey (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.RepoGetKey (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.RepoGetKey (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.RepoDeleteKey (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.RepoDeleteKey (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.IssueListLabels (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4) =
            this.IssueListLabels (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4)

        member this.IssueCreateLabel (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.IssueCreateLabel (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.IssueGetLabel (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.IssueGetLabel (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.IssueDeleteLabel (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.IssueDeleteLabel (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.IssueEditLabel (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4) =
            this.IssueEditLabel (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4)

        member this.RepoGetLanguages (arg_0_0, arg_0_1, arg_0_2) =
            this.RepoGetLanguages (arg_0_0, arg_0_1, arg_0_2)

        member this.RepoGetRawFileOrLFS (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4) =
            this.RepoGetRawFileOrLFS (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4)

        member this.IssueGetMilestonesList (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4, arg_0_5, arg_0_6) =
            this.IssueGetMilestonesList (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4, arg_0_5, arg_0_6)

        member this.IssueCreateMilestone (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.IssueCreateMilestone (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.IssueGetMilestone (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.IssueGetMilestone (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.IssueDeleteMilestone (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.IssueDeleteMilestone (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.IssueEditMilestone (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4) =
            this.IssueEditMilestone (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4)

        member this.RepoMirrorSync (arg_0_0, arg_0_1, arg_0_2) =
            this.RepoMirrorSync (arg_0_0, arg_0_1, arg_0_2)

        member this.NotifyGetRepoList
            (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4, arg_0_5, arg_0_6, arg_0_7, arg_0_8, arg_0_9)
            =
            this.NotifyGetRepoList (
                arg_0_0,
                arg_0_1,
                arg_0_2,
                arg_0_3,
                arg_0_4,
                arg_0_5,
                arg_0_6,
                arg_0_7,
                arg_0_8,
                arg_0_9
            )

        member this.NotifyReadRepoList (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4, arg_0_5, arg_0_6) =
            this.NotifyReadRepoList (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4, arg_0_5, arg_0_6)

        member this.RepoListPullRequests
            (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4, arg_0_5, arg_0_6, arg_0_7, arg_0_8)
            =
            this.RepoListPullRequests (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4, arg_0_5, arg_0_6, arg_0_7, arg_0_8)

        member this.RepoCreatePullRequest (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.RepoCreatePullRequest (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.RepoGetPullRequest (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.RepoGetPullRequest (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.RepoEditPullRequest (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4) =
            this.RepoEditPullRequest (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4)

        member this.RepoDownloadPullDiffOrPatch (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4, arg_0_5) =
            this.RepoDownloadPullDiffOrPatch (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4, arg_0_5)

        member this.RepoGetPullRequestCommits (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4, arg_0_5) =
            this.RepoGetPullRequestCommits (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4, arg_0_5)

        member this.RepoGetPullRequestFiles (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4, arg_0_5, arg_0_6, arg_0_7) =
            this.RepoGetPullRequestFiles (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4, arg_0_5, arg_0_6, arg_0_7)

        member this.RepoPullRequestIsMerged (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.RepoPullRequestIsMerged (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.RepoMergePullRequest (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4) =
            this.RepoMergePullRequest (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4)

        member this.RepoCancelScheduledAutoMerge (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.RepoCancelScheduledAutoMerge (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.RepoCreatePullReviewRequests (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4) =
            this.RepoCreatePullReviewRequests (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4)

        member this.RepoDeletePullReviewRequests (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4) =
            this.RepoDeletePullReviewRequests (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4)

        member this.RepoListPullReviews (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4, arg_0_5) =
            this.RepoListPullReviews (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4, arg_0_5)

        member this.RepoCreatePullReview (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4) =
            this.RepoCreatePullReview (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4)

        member this.RepoGetPullReview (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4) =
            this.RepoGetPullReview (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4)

        member this.RepoSubmitPullReview (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4, arg_0_5) =
            this.RepoSubmitPullReview (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4, arg_0_5)

        member this.RepoDeletePullReview (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4) =
            this.RepoDeletePullReview (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4)

        member this.RepoGetPullReviewComments (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4) =
            this.RepoGetPullReviewComments (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4)

        member this.RepoDismissPullReview (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4, arg_0_5) =
            this.RepoDismissPullReview (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4, arg_0_5)

        member this.RepoUnDismissPullReview (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4) =
            this.RepoUnDismissPullReview (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4)

        member this.RepoUpdatePullRequest (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4) =
            this.RepoUpdatePullRequest (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4)

        member this.RepoListPushMirrors (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4) =
            this.RepoListPushMirrors (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4)

        member this.RepoAddPushMirror (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.RepoAddPushMirror (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.RepoPushMirrorSync (arg_0_0, arg_0_1, arg_0_2) =
            this.RepoPushMirrorSync (arg_0_0, arg_0_1, arg_0_2)

        member this.RepoGetPushMirrorByRemoteName (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.RepoGetPushMirrorByRemoteName (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.RepoDeletePushMirror (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.RepoDeletePushMirror (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.RepoGetRawFile (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4) =
            this.RepoGetRawFile (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4)

        member this.RepoListReleases (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4, arg_0_5, arg_0_6, arg_0_7) =
            this.RepoListReleases (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4, arg_0_5, arg_0_6, arg_0_7)

        member this.RepoCreateRelease (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.RepoCreateRelease (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.RepoGetLatestRelease (arg_0_0, arg_0_1, arg_0_2) =
            this.RepoGetLatestRelease (arg_0_0, arg_0_1, arg_0_2)

        member this.RepoGetReleaseByTag (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.RepoGetReleaseByTag (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.RepoDeleteReleaseByTag (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.RepoDeleteReleaseByTag (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.RepoGetRelease (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.RepoGetRelease (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.RepoDeleteRelease (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.RepoDeleteRelease (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.RepoEditRelease (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4) =
            this.RepoEditRelease (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4)

        member this.RepoListReleaseAttachments (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.RepoListReleaseAttachments (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.RepoGetReleaseAttachment (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4) =
            this.RepoGetReleaseAttachment (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4)

        member this.RepoDeleteReleaseAttachment (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4) =
            this.RepoDeleteReleaseAttachment (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4)

        member this.RepoEditReleaseAttachment (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4, arg_0_5) =
            this.RepoEditReleaseAttachment (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4, arg_0_5)

        member this.RepoGetReviewers (arg_0_0, arg_0_1, arg_0_2) =
            this.RepoGetReviewers (arg_0_0, arg_0_1, arg_0_2)

        member this.RepoSigningKey (arg_0_0, arg_0_1, arg_0_2) =
            this.RepoSigningKey (arg_0_0, arg_0_1, arg_0_2)

        member this.RepoListStargazers (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4) =
            this.RepoListStargazers (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4)

        member this.RepoListStatuses (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4, arg_0_5, arg_0_6, arg_0_7) =
            this.RepoListStatuses (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4, arg_0_5, arg_0_6, arg_0_7)

        member this.RepoCreateStatus (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4) =
            this.RepoCreateStatus (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4)

        member this.RepoListSubscribers (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4) =
            this.RepoListSubscribers (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4)

        member this.UserCurrentCheckSubscription (arg_0_0, arg_0_1, arg_0_2) =
            this.UserCurrentCheckSubscription (arg_0_0, arg_0_1, arg_0_2)

        member this.UserCurrentDeleteSubscription (arg_0_0, arg_0_1, arg_0_2) =
            this.UserCurrentDeleteSubscription (arg_0_0, arg_0_1, arg_0_2)

        member this.UserCurrentPutSubscription (arg_0_0, arg_0_1, arg_0_2) =
            this.UserCurrentPutSubscription (arg_0_0, arg_0_1, arg_0_2)

        member this.RepoListTags (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4) =
            this.RepoListTags (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4)

        member this.RepoCreateTag (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.RepoCreateTag (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.RepoGetTag (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.RepoGetTag (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.RepoDeleteTag (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.RepoDeleteTag (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.RepoListTeams (arg_0_0, arg_0_1, arg_0_2) =
            this.RepoListTeams (arg_0_0, arg_0_1, arg_0_2)

        member this.RepoCheckTeam (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.RepoCheckTeam (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.RepoDeleteTeam (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.RepoDeleteTeam (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.RepoAddTeam (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.RepoAddTeam (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.RepoTrackedTimes (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4, arg_0_5, arg_0_6, arg_0_7) =
            this.RepoTrackedTimes (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4, arg_0_5, arg_0_6, arg_0_7)

        member this.UserTrackedTimes (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.UserTrackedTimes (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.RepoListTopics (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4) =
            this.RepoListTopics (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4)

        member this.RepoUpdateTopics (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.RepoUpdateTopics (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.RepoDeleteTopic (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.RepoDeleteTopic (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.RepoAddTopic (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.RepoAddTopic (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.RepoTransfer (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.RepoTransfer (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.AcceptRepoTransfer (arg_0_0, arg_0_1, arg_0_2) =
            this.AcceptRepoTransfer (arg_0_0, arg_0_1, arg_0_2)

        member this.RejectRepoTransfer (arg_0_0, arg_0_1, arg_0_2) =
            this.RejectRepoTransfer (arg_0_0, arg_0_1, arg_0_2)

        member this.RepoCreateWikiPage (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.RepoCreateWikiPage (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.RepoGetWikiPage (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.RepoGetWikiPage (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.RepoDeleteWikiPage (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.RepoDeleteWikiPage (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.RepoEditWikiPage (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4) =
            this.RepoEditWikiPage (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4)

        member this.RepoGetWikiPages (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4) =
            this.RepoGetWikiPages (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4)

        member this.RepoGetWikiPageRevisions (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4) =
            this.RepoGetWikiPageRevisions (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4)

        member this.GenerateRepo (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.GenerateRepo (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.RepoGetByID (arg_0_0, arg_0_1) = this.RepoGetByID (arg_0_0, arg_0_1)
        member this.GetGeneralAPISettings arg_0_0 = this.GetGeneralAPISettings (arg_0_0)

        member this.GetGeneralAttachmentSettings arg_0_0 =
            this.GetGeneralAttachmentSettings (arg_0_0)

        member this.GetGeneralRepositorySettings arg_0_0 =
            this.GetGeneralRepositorySettings (arg_0_0)

        member this.GetGeneralUISettings arg_0_0 = this.GetGeneralUISettings (arg_0_0)
        member this.GetSigningKey arg_0_0 = this.GetSigningKey (arg_0_0)
        member this.OrgGetTeam (arg_0_0, arg_0_1) = this.OrgGetTeam (arg_0_0, arg_0_1)
        member this.OrgDeleteTeam (arg_0_0, arg_0_1) = this.OrgDeleteTeam (arg_0_0, arg_0_1)

        member this.OrgEditTeam (arg_0_0, arg_0_1, arg_0_2) =
            this.OrgEditTeam (arg_0_0, arg_0_1, arg_0_2)

        member this.OrgListTeamMembers (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.OrgListTeamMembers (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.OrgListTeamMember (arg_0_0, arg_0_1, arg_0_2) =
            this.OrgListTeamMember (arg_0_0, arg_0_1, arg_0_2)

        member this.OrgRemoveTeamMember (arg_0_0, arg_0_1, arg_0_2) =
            this.OrgRemoveTeamMember (arg_0_0, arg_0_1, arg_0_2)

        member this.OrgAddTeamMember (arg_0_0, arg_0_1, arg_0_2) =
            this.OrgAddTeamMember (arg_0_0, arg_0_1, arg_0_2)

        member this.OrgListTeamRepos (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.OrgListTeamRepos (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.OrgListTeamRepo (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.OrgListTeamRepo (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.OrgRemoveTeamRepository (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.OrgRemoveTeamRepository (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.OrgAddTeamRepository (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.OrgAddTeamRepository (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.TopicSearch (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.TopicSearch (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.UserGetCurrent arg_0_0 = this.UserGetCurrent (arg_0_0)

        member this.UserGetOauth2Application (arg_0_0, arg_0_1, arg_0_2) =
            this.UserGetOauth2Application (arg_0_0, arg_0_1, arg_0_2)

        member this.UserCreateOAuth2Application (arg_0_0, arg_0_1) =
            this.UserCreateOAuth2Application (arg_0_0, arg_0_1)

        member this.UserGetOAuth2Application (arg_0_0, arg_0_1) =
            this.UserGetOAuth2Application (arg_0_0, arg_0_1)

        member this.UserDeleteOAuth2Application (arg_0_0, arg_0_1) =
            this.UserDeleteOAuth2Application (arg_0_0, arg_0_1)

        member this.UserUpdateOAuth2Application (arg_0_0, arg_0_1, arg_0_2) =
            this.UserUpdateOAuth2Application (arg_0_0, arg_0_1, arg_0_2)

        member this.UserListEmails arg_0_0 = this.UserListEmails (arg_0_0)
        member this.UserAddEmail (arg_0_0, arg_0_1) = this.UserAddEmail (arg_0_0, arg_0_1)
        member this.UserDeleteEmail (arg_0_0, arg_0_1) = this.UserDeleteEmail (arg_0_0, arg_0_1)

        member this.UserCurrentListFollowers (arg_0_0, arg_0_1, arg_0_2) =
            this.UserCurrentListFollowers (arg_0_0, arg_0_1, arg_0_2)

        member this.UserCurrentListFollowing (arg_0_0, arg_0_1, arg_0_2) =
            this.UserCurrentListFollowing (arg_0_0, arg_0_1, arg_0_2)

        member this.UserCurrentCheckFollowing (arg_0_0, arg_0_1) =
            this.UserCurrentCheckFollowing (arg_0_0, arg_0_1)

        member this.UserCurrentDeleteFollow (arg_0_0, arg_0_1) =
            this.UserCurrentDeleteFollow (arg_0_0, arg_0_1)

        member this.UserCurrentPutFollow (arg_0_0, arg_0_1) =
            this.UserCurrentPutFollow (arg_0_0, arg_0_1)

        member this.GetVerificationToken arg_0_0 = this.GetVerificationToken (arg_0_0)

        member this.UserCurrentDeleteGPGKey (arg_0_0, arg_0_1) =
            this.UserCurrentDeleteGPGKey (arg_0_0, arg_0_1)

        member this.UserCurrentListKeys (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.UserCurrentListKeys (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.UserCurrentPostKey (arg_0_0, arg_0_1) =
            this.UserCurrentPostKey (arg_0_0, arg_0_1)

        member this.UserCurrentGetKey (arg_0_0, arg_0_1) =
            this.UserCurrentGetKey (arg_0_0, arg_0_1)

        member this.UserCurrentDeleteKey (arg_0_0, arg_0_1) =
            this.UserCurrentDeleteKey (arg_0_0, arg_0_1)

        member this.OrgListCurrentUserOrgs (arg_0_0, arg_0_1, arg_0_2) =
            this.OrgListCurrentUserOrgs (arg_0_0, arg_0_1, arg_0_2)

        member this.UserCurrentListRepos (arg_0_0, arg_0_1, arg_0_2) =
            this.UserCurrentListRepos (arg_0_0, arg_0_1, arg_0_2)

        member this.CreateCurrentUserRepo (arg_0_0, arg_0_1) =
            this.CreateCurrentUserRepo (arg_0_0, arg_0_1)

        member this.GetUserSettings arg_0_0 = this.GetUserSettings (arg_0_0)

        member this.UpdateUserSettings (arg_0_0, arg_0_1) =
            this.UpdateUserSettings (arg_0_0, arg_0_1)

        member this.UserCurrentListStarred (arg_0_0, arg_0_1, arg_0_2) =
            this.UserCurrentListStarred (arg_0_0, arg_0_1, arg_0_2)

        member this.UserCurrentCheckStarring (arg_0_0, arg_0_1, arg_0_2) =
            this.UserCurrentCheckStarring (arg_0_0, arg_0_1, arg_0_2)

        member this.UserCurrentDeleteStar (arg_0_0, arg_0_1, arg_0_2) =
            this.UserCurrentDeleteStar (arg_0_0, arg_0_1, arg_0_2)

        member this.UserCurrentPutStar (arg_0_0, arg_0_1, arg_0_2) =
            this.UserCurrentPutStar (arg_0_0, arg_0_1, arg_0_2)

        member this.UserGetStopWatches (arg_0_0, arg_0_1, arg_0_2) =
            this.UserGetStopWatches (arg_0_0, arg_0_1, arg_0_2)

        member this.UserCurrentListSubscriptions (arg_0_0, arg_0_1, arg_0_2) =
            this.UserCurrentListSubscriptions (arg_0_0, arg_0_1, arg_0_2)

        member this.UserListTeams (arg_0_0, arg_0_1, arg_0_2) =
            this.UserListTeams (arg_0_0, arg_0_1, arg_0_2)

        member this.UserCurrentTrackedTimes (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4) =
            this.UserCurrentTrackedTimes (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4)

        member this.UserSearch (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4) =
            this.UserSearch (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4)

        member this.UserGet (arg_0_0, arg_0_1) = this.UserGet (arg_0_0, arg_0_1)

        member this.UserListFollowers (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.UserListFollowers (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.UserListFollowing (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.UserListFollowing (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.UserCheckFollowing (arg_0_0, arg_0_1, arg_0_2) =
            this.UserCheckFollowing (arg_0_0, arg_0_1, arg_0_2)

        member this.UserGetHeatmapData (arg_0_0, arg_0_1) =
            this.UserGetHeatmapData (arg_0_0, arg_0_1)

        member this.UserListKeys (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4) =
            this.UserListKeys (arg_0_0, arg_0_1, arg_0_2, arg_0_3, arg_0_4)

        member this.OrgListUserOrgs (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.OrgListUserOrgs (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.OrgGetUserPermissions (arg_0_0, arg_0_1, arg_0_2) =
            this.OrgGetUserPermissions (arg_0_0, arg_0_1, arg_0_2)

        member this.UserListRepos (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.UserListRepos (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.UserListStarred (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.UserListStarred (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.UserListSubscriptions (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.UserListSubscriptions (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.UserGetTokens (arg_0_0, arg_0_1, arg_0_2, arg_0_3) =
            this.UserGetTokens (arg_0_0, arg_0_1, arg_0_2, arg_0_3)

        member this.UserCreateToken (arg_0_0, arg_0_1, arg_0_2) =
            this.UserCreateToken (arg_0_0, arg_0_1, arg_0_2)

        member this.UserDeleteAccessToken (arg_0_0, arg_0_1, arg_0_2) =
            this.UserDeleteAccessToken (arg_0_0, arg_0_1, arg_0_2)

        member this.GetVersion arg_0_0 = this.GetVersion (arg_0_0)
