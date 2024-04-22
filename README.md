string original = "2131513/1513";
int lastIndex = original.LastIndexOf('/');
string result = (lastIndex >= 0) ? original.Remove(lastIndex, 1) : original;
