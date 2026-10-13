export interface User {
  userId: string;
  id: string;
  userName: string;
  fullName: string;
  avatar: string;
  userGroupName: string;
  isAdmin: boolean;
  token: string;
  expireTime: number;
  listMenu: any[];
}
