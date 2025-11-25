import { makeAutoObservable, runInAction } from "mobx";
import {
  CreateUserModel,
  UserFormValues,
  UserWithLocale,
} from "../interfaces/user.interface";
import { history } from "../";
import { store } from "./store";
import userAgent from "../helpers/api/userAgent";

export default class UserStore {
  user: UserWithLocale | null = null;

  constructor() {
    makeAutoObservable(this);
  }

  get isLoggedIn() {
    return !!this.user;
  }

  login = async (creds: UserFormValues) => {
    try {
      const token = await userAgent.Account.login(creds);
      store.commonStore.setToken(token);
      //this.startRefreshTokenTimer(user);
      this.setUser();
      //runInAction(() => this.user = user);
      history.push("/");
      store.modalStore.closeModal();
    } catch (error) {
      throw error;
    }
  };

  logout = () => {
    store.commonStore.setToken(null);
    window.localStorage.removeItem("jwt");
    this.user = null;
    history.push("/");
  };

  setUser = async () => {
    try {
      const user = await userAgent.Account.current();
      const userInfo = await userAgent.Account.userInfo(
        store.commonStore.token!
      );
      //store.commonStore.setToken(user.token);
      runInAction(() => {
        this.user = { ...user, roles: userInfo.roles };
      });
    } catch (error) {
      console.log(error);
    }
  };

  register = async (creds: CreateUserModel) => {
    try {
      await userAgent.Account.register(creds);
      store.modalStore.closeModal();
    } catch (error) {
      throw error;
    }
  };
}
