using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace citools
{

    public enum StoreResourceLogDtoState
    {
        Read,
        Write,
        Delete
    }

    public class StoreResourceLogDto
    {

        public StoreResourceLogDtoState State { get; }
        public string Name {get; set;}

        public StoreResourceLogDto()
        {
        }

        public StoreResourceLogDto(StoreResource resource, StoreResourceLogDtoState state)
        {
            this.Name = resource.Path();
            State = state;
        }

    }
}