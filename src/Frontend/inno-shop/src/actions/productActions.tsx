import type { ActionFunctionArgs } from "react-router";

export const newProductAction = async ({ request }: ActionFunctionArgs) => {
  console.log("Action");
  return null;
};
