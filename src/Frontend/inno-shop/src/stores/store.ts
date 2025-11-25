import { createContext, useContext } from "react";
import CommonStore from "./commonStore";
import UserStore from "./userStore";
import ProductStore from "./productStore";

interface Store {
  productStore: ProductStore;
  userStore: UserStore;
  commonStore: CommonStore;
}

export const store: Store = {
  productStore: new ProductStore(),
  userStore: new UserStore(),
  commonStore: new CommonStore(),
};

export const StoreContext = createContext(store);

export function useStore() {
  return useContext(StoreContext);
}
