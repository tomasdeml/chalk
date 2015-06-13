# chalk

Chalk is a command-line utility for export of commits from SourceGear Vault repository to Git. In comparison to https://github.com/AndreyNikiforov/vault2git Chalk uses Vault command line client (instead of Vault API) and also supports export from a specific version (instead of beginning on the first repository revision).

Chalk is not optimized for speed (yet) as it's primary use case is to export commits nightly to a Phabricator-managed Git repository. This way it is possible to perform code reviews even when there is no code review tool supporting aging SourceGear Vault.

## Known issues
- A commit with message containing double-quotes (") in Vault, will crash chalk - this is a bug in the Vault command line client that generates invalid XML (i.e. it does not escape quotes) when such message is present.

**Beware - this is an alpha version**

## Licence
Apache 2, see LICENCE.txt.
