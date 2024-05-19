using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace csprj.leetcode {
    public class Solution {
        public bool IsPossibleDivide(int[] nums, int k) {
            if (nums.Length % k != 0) return false;
            var map = new SortedDictionary<int, int>();
            foreach (var n in nums) {
                map[n] = map.GetValueOrDefault(n, 0) + 1;
            }

            while (map.Count > 0) {
                var cur = map.Min((x)=>x.Key);
                for (int i = 0; i < k; ++i) {
                    if (!map.ContainsKey(cur)) return false;
                    map[cur]--;
                    if (map[cur] == 0) map.Remove(cur);
                    cur++;
                }
            }

            return true;
        }
    }
    public class SolutionPivotIndex {
        public int PivotIndex(int[] nums) {
            var sum = nums.Sum();
            int accSum = 0;
            for (int i = 0; i < nums.Length; ++i) {
                if ((sum - nums[i]) % 2 == 0 && (sum - nums[i]) / 2 == accSum) return i;
                accSum += nums[i];
            }

            return -1;
        }
    }
    public class SolutionMergeKLists {
        public ListNode MergeKLists(ListNode[] lists) {
            if (lists == null || lists.Length == 0) return null;
            var curLists = new List<ListNode>(lists);
            while (curLists.Count > 1) {
                var newLists = new List<ListNode>();
                for (int i = 0; i < curLists.Count; i += 2) {
                    if (i == curLists.Count - 1) {
                        newLists.Add(curLists[i]);
                    }
                    else {
                        newLists.Add(Merge2Lists(curLists[i],curLists[i+1]));
                    }
                }
            }

            return curLists[0];
        }

        ListNode Merge2Lists(ListNode l1, ListNode l2) {
            if (l1 == null) return l2;
            if (l2 == null) return l1;
            if (l1.val < l2.val) {
                l1.next = Merge2Lists(l1.next, l2);
                return l1;
            }

            l2.next = Merge2Lists(l1, l2.next);
            return l2;
        }
    }
    public class SolutionFindMaximizedCapital {
        public int FindMaximizedCapital(int k, int w, int[] profits, int[] capital) {
            var cap_pro_pairs = new List<(int,int)>(capital.Zip(profits));
            cap_pro_pairs.Sort((x,y)=>y.Item2-x.Item2);
            var cap_pro_used = new List<bool>(cap_pro_pairs.Count);
            for (int i = 0; i < cap_pro_used.Count; ++i) {
                cap_pro_used.Add(false);
            }
            
            while (k>0) {
                k--;
                for (int i = 0; i < cap_pro_pairs.Count; ++i) {
                    var (c, p) = cap_pro_pairs[i];
                    if (c <= w && !cap_pro_used[i]) {
                        cap_pro_used[i] = true;
                        w += p;
                    }
                }
            }

            return w;

        }
    }
    public class SolutionMinSubarray {
        public int MinSubarray(int[] nums, int p) {
            var sum = nums.Sum();
            var rest = sum % p;
            if (rest == 0) return 0;
            var acc_sums = new List<int>(nums.Length);
            var cur = 0;
            foreach (var e in nums) {
                cur += e;
                acc_sums.Add(cur);
            }

            int res = nums.Length;

            for (int i = 0; i < acc_sums.Count; ++i) {
                for (int j = i; j < acc_sums.Count; ++j) {
                    var subsum = 0;
                    if (i == 0) subsum = acc_sums[j];
                    else subsum = acc_sums[j] - acc_sums[i - 1];
                    var cur_len = j - i + 1;
                    if (subsum % p == rest && cur_len < res) res = cur_len;
                }
            }

            return res;
        }
    }
    public class Solution1 {
        public bool ValidPalindrome(string s) {
            bool deleted = false;
            bool func(int lo, int hi) {
                if (hi - lo <= 1) return true;
                if (s[lo] != s[hi - 1]) {
                    if (deleted) return false;
                    deleted = true;
                    var a = func(lo, hi - 1);
                    var b = func(lo + 1, hi);
                    return a || b;
                }
                return func(lo + 1, hi - 1);
            }

            return func(0, s.Length);
        }
    }
}