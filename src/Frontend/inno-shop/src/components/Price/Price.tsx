import { Typography } from "@mui/material";

interface props {
  price: number;
  salePrice: number;
  isOnSale: boolean;
}

interface props {
  price: number;
  salePrice: number;
  isOnSale: boolean;
}

const Price = ({ price, salePrice, isOnSale }: props) => {
  if (!isOnSale) return <Typography>{price} $</Typography>;
  else
    return (
      <>
        <Typography sx={{ textDecoration: "line-through" }}>
          {price} $
        </Typography>
        <Typography color="red">{salePrice} $</Typography>
      </>
    );
};

export default Price;
