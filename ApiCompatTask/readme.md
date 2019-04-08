# ApiCompat tool for Azure Pipeline



## Task Details

| Field                                | Details                                                      |
| ------------------------------------ | ------------------------------------------------------------ |
| **Contracts Root Folder**            | **Required.** Path to folder that contains the binaries that will be used to check your newly built assemblies for incompatibilities. |
| **Contracts Files Name**             | **Required.** Name of the contract binaries files, including file extension. You may specify more than one file separating them by a space. |
| **Implementation Assemblies Folder** | **Required.** Path to the folder where your newly built assemblies reside. |
| **Fails on Issues**                  | If checked, when the ApiCompat tool finds one or more incompatibilities between the new assemblies and the contract assemblies, the task will be marked as *failed*. Use this if you want the build to fail when issues are found. Otherwise, the task will be marked as *successful with issues*. |
| **Resolve Fx**                       | If a contract or implementation dependency cannot be found in the given directories, fallback to try to resolve against the framework directory on the machine. |
| **Warn on incorrect version**        | Warn if the contract version number doesn't match the found implementation version number. |
| **Warn on missing assemblies**       | Warn if the contract assembly cannot be found in the implementation directories. Default is to error and not do analysis. |
| **Create Comparison result log**     | If checked, a JSON file will be created with two properties: `issues` contains the number of incompatibilities, and `body` contains the ApiCompat result. If not incompatibilities are found, a JSON with a message saying no issues were found will be created instead. |
| **Output file name**                 | *Only visible when* `Create Comparison result log` *is checked*. Name of the JSON file to create, including extension. |
| **Output file path**                 | *Only visible when* `Create Comparison result log` *is checked*. Path to the folder where the result log will be generated. |
| **Use baseline file**                | If checked, it will use a specified baseline file with known incompatibilities to be ignored when comparing. |
| **Baseline file path**               | *Only visible when* `Use baseline file` *is checked*. Path to the file that contains the known differences. |

