﻿namespace ClassLibrary_SEP3
{
    public class Backlog
    {
        public int BacklogID { get; init; }
        public int ProjectID { get; init; }
        public string Description { get;set; }

        
        
        //Change later
        public Backlog(string description)
        {
            Description = description;
        }

        public Backlog()
        {
            throw new NotImplementedException();
        }
    }
}
