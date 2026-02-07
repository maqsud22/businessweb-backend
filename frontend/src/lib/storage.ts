export const storage = {
  getToken: () => localStorage.getItem('bw_token'),
  setToken: (token: string) => localStorage.setItem('bw_token', token),
  clearToken: () => localStorage.removeItem('bw_token')
};
