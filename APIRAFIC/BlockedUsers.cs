
namespace APIRAFIC
{
    public class BlockedUsers
    {
        Dictionary<int, byte> users = new Dictionary<int, byte>();

        internal byte AddBadLogin(int id)
        {
            if (users.ContainsKey(id))
            {
                users[id]++;
                if (users[id] > 3)
                    users[id] = 3;
            }
            else
                users.Add(id, 1);
            return users[id];
        }
    }
}
