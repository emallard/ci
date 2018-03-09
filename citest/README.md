# citest

A test is a Program and not a XUnit test.

It's made of multiple steps. Each steps can be long so thegoal is to not do everything each time.

So each Step is made of 3 methods :
- Test : Does the step need to be done ?
- Run : Do it
- Clean : Delete what is needed, so that we can really test the step with Run.

Loop to run tests:
``` cs
try {
    step.Test();
}
catch (Exception)
{
    step.Clean();
    step.Run();
    step.Test();
}
```

# Examples of tests :

- VmPilote_1_Create
- VmPilote_1_Hosts
- VmPilote_2_Docker
- VmPilote_3_MirrorRegistry
- VmPilote_5_PiloteCi_Build
- VmPilote_4_PiloteCi_Sources

- PiloteCi_1_InstallCA
- PiloteCi_3_InstallPrivateRegistry
- PiloteCi_1_Build
- PiloteCi_2_Publish