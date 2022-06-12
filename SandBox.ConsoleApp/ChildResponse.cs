namespace SandBox.ConsoleApp
{
    //public record ChildResponse(string Name);
    //public class ChildResponse
    //{
    //    public ChildResponse(int id, string firstName, string secondName, string email)
    //    {
    //        Id = id;
    //        FirstName = firstName;
    //        SecondName = secondName;
    //        Email = email;
    //    }

    //    public ChildResponse()
    //    {

    //    }

    //    public int Id { get; set; }
    //    public string FirstName { get; set; }
    //    public string SecondName { get; set; }
    //    public string Email { get; set; }
    //}

    //public class ChildResponse
    //{
    //    public ChildResponse(int id)
    //    {
    //        Id = id;
    //    }

    //    public ChildResponse()
    //    {

    //    }

    //    public int Id { get; set; }
    //}

    public class ChildResponse
    {

        public ChildResponse(string name)
        {
            this.Name = name;
        }

        //public ChildResponse()
        //{

        //}

        public string Name { get; set; }
    }
}