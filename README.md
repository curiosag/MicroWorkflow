# MicroWorkflow

A study to use Linq syntax to express data processing sequences that could be suspended, persisted, de-serialized and resumed. The idea is, to track the execution state in the background and use it in case of a suspend/resume. In that case the functions behind the Linq expression will get called once more, but for those steps already covered their assigned actions won't execute again.  
