namespace csprj.leetcode {
    public class Solution {
        public bool ValidPalindrome(string s) {
            var res = Valid(s);
            if (res) return true;
            for (int i = 0; i < s.Length; ++i) {
                var r = Valid(s.Substring(0, i - 1) + s.Substring(i + 1));
                if (r) return true;
            }

            return false;
        }
        public bool Valid(string s) {
            if (s.Length <= 1) return true;
            if (s[0] != s[^1]) {
                return false;
            }

            return Valid(s.Substring(1, s.Length - 2));
        }
    }
}