namespace A2.Data
{
    public class A2Repo : IA2Repo
    {
        private readonly A2DbContext _dbContext;

        public A2Repo(A2DbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}