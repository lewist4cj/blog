import { type BaseResponse, http } from './index'

export interface SiteInfoResponse {
  siteInfo: { title: string; logo: string; beian: string; mode: number }
  project: { title: string; icon: string; webPath: string }
  seo: { keywords: string; description: string }
  about: {
    siteDate: string
    version: string
    qq: string
    wechat: string
    github: string
    gitee: string
    bilibili: string
  }
  login: { qqLogin: boolean; usernamePwdLogin: boolean; emailLogin: boolean; captcha: boolean }
  article: { noExamine: boolean }
}

export interface EmailSettings {
  domain: string
  port: number
  sendEmail: string
  authCode: string
  sendNickname: string
  ssl: boolean
  tls: boolean
}

export interface QqSettings {
  appID: string
  appKey: string
  redirect: string
}

export interface QiNiuSettings {
  enable: boolean
  accessKey: string
  secretKey: string
  bucket: string
  uri: string
  region: string
  prefix: string
  size: number
  expiry: number
}

export interface AiSettings {
  enable: boolean
  secretKey: string
  nickname: string
  avatar: string
  abstract: string
}

export type SiteName = 'site' | 'email' | 'qq' | 'qiNiu' | 'ai'

export function siteApi<T extends SiteName>(name: T): Promise<BaseResponse<Record<string, unknown>>> {
  return http.get(`/api/site/info?name=${name}`)
}

export function siteUpdateApi<T extends SiteName>(
  name: T,
  data: Record<string, unknown>,
): Promise<BaseResponse<string>> {
  return http.put(`/api/site/info?name=${name}`, data)
}

export function siteRedirectionApi(): Promise<BaseResponse<string>> {
  return http.get('/api/site/redirection')
}
