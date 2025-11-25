import {
  Form,
  useLoaderData,
  useLocation,
  useNavigate,
  useParams,
} from "react-router";
import type { ProductDetail } from "../interfaces/product.interface";
import Price from "../components/Price/Price";
import type { loadProductDetail } from "../loaders/productLoader";
import { Button } from "@mui/material";

export const ProductDetailPage = () => {
  const location = useLocation();
  const navigate = useNavigate();
  const { currentPage } = location.state || {};
  const { productId } = useParams<{ productId: string }>();
  const { product } = useLoaderData<typeof loadProductDetail>();
  if (!product) {
    return <div>Loading...</div>;
  }
  // const { items } = routeData;
  // const product = items.find(({ id }) => id.toString() === productId);

  // if (!product) {
  //   return <div>Product not found</div>;
  // }
  return (
    <div>
      <p>{product.categoryName}</p>
      <h2>{product.name}</h2>
      {/* <img src={product.image} alt={product.name} width={200} /> */}
      <p>{product.description}</p>
      <Price
        price={product.price}
        isOnSale={product.isOnSale}
        salePrice={product.salePrice}
      />
      <Form method="DELETE">
        <input type="hidden" name="id" value={product.id} />
        <Button type="submit">Delete</Button>
      </Form>
      <Button onClick={() => navigate(-1, { state: { currentPage } })}>
        Back to list
      </Button>
    </div>
  );
};

export default ProductDetailPage;
