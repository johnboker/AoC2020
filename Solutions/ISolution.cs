using System.Threading.Tasks;

namespace AoC2020.Solutions
{
    public interface ISolution
    {
        Task ReadInput(string file);
        void Solve1();
        void Solve2();
    }
}