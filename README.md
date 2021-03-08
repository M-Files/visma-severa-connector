This project implements a Visma Severa -connector reference implementation for the purposes of demonstrating the use of M-Files's External Object Type APIs. The implementation allows exposing data in Visma Severa system as external objects in M-Files.

# Using the sample project

1. Prepare the project

	a. Add a reference to `MFiles.Server.Extensions.dll` to the provided project.  This file can be found in your M-Files installation (e.g. `C:\Program Files\M-Files\<VersionNumber>\Bin\anycpu`).
	b. Build the project, ensuring that there are no errors reported.

2. Install the connector to the M-Files server

	a. Copy files from MFiles.Server.Extensions.dll and VismaSeveraConnector.dll to a directory

	b. Change the value "Path" in the settings file `settings.reg` to match the above directory. Be sure to ensure that the path ends with a slash ("\\").

	c. Configure the [external object type](https://www.m-files.com/user-guide/latest/eng/Legacy_Connection_to_external_database.html).

# Configuring the external object type

Below is an example connection string:

```
M-Files extension={4DD9712E-522A-4E22-9A89-12A88C3B4045};endpoint=https://sync.severa.com/webservice/S3/API.svc/;apikey=[replace-this-withapi-key]
```

The select statement should be one of the following formats:

SELECT statements:		
- `type=account;columns=*`
- `type=case;columns=*`
- `type=contact;columns=*`
- `type=product;columns=*`
- `type=product category;columns=*`		(value list)

*Note, that this implementation is only for one way connectivity and insert and update statements should be disabled.*
