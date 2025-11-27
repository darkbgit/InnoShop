import type { ProductDetail } from "../../interfaces/product.interface";
import Price from "../Price/Price";

const ProductDetailCard = ({ product }: { product: ProductDetail }) => {
  return (
    <div>
      <p>{product.categoryName}</p>
      <h2>{product.name}</h2>
      <p>{product.longDescription}</p>
      <Price
        price={product.price}
        isOnSale={product.isOnSale}
        salePrice={product.salePrice}
      />
    </div>
  );
};

export default ProductDetailCard;
