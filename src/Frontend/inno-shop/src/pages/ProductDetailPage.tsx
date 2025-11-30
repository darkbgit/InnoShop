import {
  Form,
  useLoaderData,
  useLocation,
  useNavigate,
  useParams,
} from "react-router";

import type { loadProductDetail } from "../loaders/productLoaders";
import { Button } from "@mui/material";
import ProductDetailCard from "../components/ProductDetailCard/ProductDetailCard";

export const ProductDetailPage = () => {
  const location = useLocation();
  const navigate = useNavigate();
  const { currentPage } = location.state || {};
  useParams<{ productId: string; }>();
  const product = useLoaderData<typeof loadProductDetail>();
  if (!product) {
    return <div>Loading...</div>;
  }
  return (
    <div>
      <ProductDetailCard product={product} />
      <Form method="DELETE">
        <input type="hidden" name="id" value={product.id} />
        <Button type="submit">Delete</Button>
      </Form>
      <Button onClick={() => navigate("..", { state: { currentPage } })}>
        Back to list
      </Button>
    </div>
  );
};

export default ProductDetailPage;
