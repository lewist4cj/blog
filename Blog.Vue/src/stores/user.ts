import { ref, computed } from 'vue'
import { defineStore } from 'pinia'
import { userLogoutApi } from '@/api/user'
import { siteApi, type SiteInfoResponse } from '@/api/site'
import { unReadMsgApi, type UnReadMsgType } from '@/api/message'
import { ElMessage } from 'element-plus'
import router from '@/router'

export interface UserInfo {
  id: number
  nickname: string
  username: string
  avatar: string
  role: number
  token: string
  lookCount: number
  articleCount: number
  fansCount: number
  followCount: number
  place: string
}

export interface SiteInfo {
  siteInfo: {
    title: string
    logo: string
    beian: string
    mode: number
  }
  project: {
    title: string
    icon: string
    webPath: string
  }
  seo: {
    keywords: string
    description: string
  }
  about: {
    siteDate: string
    version: string
    qq: string
    wechat: string
    github: string
    gitee: string
    bilibili: string
  }
  login: {
    qqLogin: boolean
    usernamePwdLogin: boolean
    emailLogin: boolean
    captcha: boolean
  }
  article: {
    noExamine: boolean
  }
}

const TOKEN_KEY = 'blog_token'
const USER_KEY = 'blog_user'

export const useUserStore = defineStore('user', () => {
  const token = ref(localStorage.getItem(TOKEN_KEY) || '')
  const userInfo = ref<UserInfo>({
    id: 0,
    nickname: '',
    username: '',
    avatar: '',
    role: 0,
    token: token.value,
    lookCount: 0,
    articleCount: 0,
    fansCount: 0,
    followCount: 0,
    place: '',
  })
  const siteInfo = ref<SiteInfoResponse | null>(null)
  const unreadMsg = ref<UnReadMsgType>({ commentMsgCount: 0, diggMsgCount: 0, privateMsgCount: 0, systemMsgCount: 0 })

  const isLogin = computed(() => !!token.value)
  const unreadTotal = computed(
    () =>
      unreadMsg.value.commentMsgCount +
      unreadMsg.value.diggMsgCount +
      unreadMsg.value.privateMsgCount +
      unreadMsg.value.systemMsgCount,
  )

  function setToken(newToken: string) {
    token.value = newToken
    userInfo.value.token = newToken
    localStorage.setItem(TOKEN_KEY, newToken)
  }

  function setUserInfo(info: Partial<UserInfo>) {
    Object.assign(userInfo.value, info)
    localStorage.setItem(USER_KEY, JSON.stringify(userInfo.value))
  }

  function loadUserInfo() {
    const saved = localStorage.getItem(USER_KEY)
    if (saved) {
      try {
        const parsed = JSON.parse(saved)
        Object.assign(userInfo.value, parsed)
        token.value = parsed.token || ''
      } catch {
        // ignore
      }
    }
  }

  async function loadSiteInfo() {
    try {
      const res = await siteApi('site')
      if (res.code === 200) siteInfo.value = res.data as unknown as SiteInfoResponse
    } catch {
      // ignore
    }
  }

  async function loadUnreadMsg() {
    if (!token.value) return
    try {
      const res = await unReadMsgApi()
      if (res.code === 200) unreadMsg.value = res.data
    } catch {
      // ignore
    }
  }

  async function logout() {
    try {
      await userLogoutApi()
    } catch {
      // ignore
    }
    token.value = ''
    userInfo.value = {
      id: 0,
      nickname: '',
      username: '',
      avatar: '',
      role: 0,
      token: '',
      lookCount: 0,
      articleCount: 0,
      fansCount: 0,
      followCount: 0,
      place: '',
    }
    localStorage.removeItem(TOKEN_KEY)
    localStorage.removeItem(USER_KEY)
    router.push('/login')
    ElMessage.success('已退出登录')
  }

  return {
    token,
    userInfo,
    siteInfo,
    unreadMsg,
    unreadTotal,
    isLogin,
    setToken,
    setUserInfo,
    loadUserInfo,
    loadSiteInfo,
    loadUnreadMsg,
    logout,
  }
})
