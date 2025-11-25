import {
  Button,
  Card,
  CardActions,
  CardContent,
  Typography,
} from "@mui/material";
import type { ProductDetail } from "../../interfaces/product.interface";
import { Link, useFetcher, useRouteLoaderData } from "react-router";
import Price from "../Price/Price";
import DeleteIcon from "@mui/icons-material/Delete";
import EditIcon from "@mui/icons-material/Edit";
import type { UserInfo } from "../../interfaces/user.interface";

interface props {
  product: ProductDetail;
}

const ProductCard = ({ product }: props) => {
  const currentUser = useRouteLoaderData("root") as UserInfo | null;
  const isOwner = currentUser && currentUser.id === product.createdBy;
  const fetcher = useFetcher();
  const isDeleting = fetcher.state === "submitting";
  return (
    <Card>
      <CardContent>
        <Typography gutterBottom variant="h5" component="div">
          {product.name}
        </Typography>
        <Typography variant="body2" sx={{ color: "text.secondary" }}>
          {product.description}
        </Typography>
        <Price
          price={product.price}
          salePrice={product.salePrice}
          isOnSale={product.isOnSale}
        />
      </CardContent>
      <CardActions>
        <Button size="small">
          <Link to={`/product/${product.id}`}>Show more</Link>
        </Button>
        {isOwner && (
          <div
            className="product-actions"
            style={{ display: "flex", gap: "10px" }}
          >
            <Button size="small" startIcon={<EditIcon />}>
              <Link to={`/product/${product.id}/edit`}>Edit</Link>
            </Button>

            <fetcher.Form
              method="delete"
              action={`/products/${product.id}/destroy`}
            >
              <Button
                size="small"
                startIcon={<DeleteIcon />}
                type="submit"
                disabled={isDeleting}
                style={{ backgroundColor: "red", color: "white" }}
              >
                {isDeleting ? "Deleting..." : "Delete"}
              </Button>
            </fetcher.Form>
          </div>
        )}
      </CardActions>
    </Card>
  );
};

export default ProductCard;
