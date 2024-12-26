export type User = {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
  isAdmin: boolean;
  password?: string;
  newPassword?: string;
  confirmNewPassword?: string;
};
