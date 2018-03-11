#netstat

https://unix.stackexchange.com/questions/9252/determining-what-process-is-bound-to-a-port

```
netstat -lnpt
```
will list the pid and process name next to each listening port. This will work under Linux, but not all others (like AIX.) Add -t if you want TCP only.
