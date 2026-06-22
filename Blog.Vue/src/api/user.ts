import { type BaseResponse, type ListResponse, type ParamsType, http } from './index'

export interface PwdLoginRequest {
  username: string
  password: string
}

export function pwdLoginApi(data: PwdLoginRequest): Promise<BaseResponse<string>> {
  return http.post('/api/user/login', data)
}

export interface UserInfoType {
  id: number
  codeAge: number
  avatar: string
  nickname: string
  username: string
  lookCount: number
  articleCount: number
  fansCount: number
  followCount: number
  place: string
  role: number
  email: string
  createdAt: string
}

export function userInfoApi(userID: number): Promise<BaseResponse<UserInfoType>> {
  return http.get('/api/user/base', { params: { id: userID } })
}

export function userLogoutApi(): Promise<BaseResponse<string>> {
  return http.delete('/api/user/logout')
}

export interface UserListType {
  id: number
  nickname: string
  username: string
  avatar: string
  ip: string
  addr: string
  articleCount: number
  createdAt: string
  lastLoginDate: string
}

export function userListApi(params: ParamsType): Promise<BaseResponse<ListResponse<UserListType>>> {
  return http.get('/api/user', { params })
}

export interface UserUpdateRequest {
  id: number
  nickname?: string
  avatar?: string
  abstract?: string
}

export function userUpdateApi(data: UserUpdateRequest): Promise<BaseResponse<string>> {
  return http.put('/api/user', data)
}

export interface UserRegisterRequest {
  username: string
  password: string
  nickname: string
}

export function userRegisterApi(data: UserRegisterRequest): Promise<BaseResponse<string>> {
  return http.post('/api/user/register', data)
}

export function userArticleTopApi(data: { articleID: number; type: number }): Promise<BaseResponse<string>> {
  return http.post('/api/article/top', data)
}
