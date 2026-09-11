import {
  Box,
  Button,
  Chip,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  Divider,
  Grid,
  Paper,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Typography,
} from '@mui/material';

export default function VeiculoDetalheModal({
  open,
  onClose,
  veiculo,
  onAdicionarProprietario,
}){
  if (!veiculo) {
    return null;
  }

  function formatarPreco(valor) {
    return Number(valor).toLocaleString('pt-BR', {
      style: 'currency',
      currency: 'BRL',
    });
  }

  function formatarData(data) {
    if (!data) {
      return 'Atual';
    }

    return new Date(data).toLocaleDateString('pt-BR');
  }

  return (
    <Dialog
      open={open}
      onClose={onClose}
      fullWidth
      maxWidth="md"
    >
      <DialogTitle>
        Detalhes do veículo
      </DialogTitle>

      <DialogContent dividers>
        <Box
          sx={{
            display: 'flex',
            justifyContent: 'space-between',
            alignItems: {
              xs: 'flex-start',
              sm: 'center',
            },
            flexDirection: {
              xs: 'column',
              sm: 'row',
            },
            gap: 1,
            mb: 3,
          }}
        >
          <Typography variant="h5" fontWeight={700}>
            {veiculo.marca} {veiculo.modelo}
          </Typography>

          <Chip
            label={veiculo.situacao}
            color={
              veiculo.situacao === 'Disponível'
                ? 'success'
                : veiculo.situacao === 'Reservado'
                  ? 'warning'
                  : 'default'
            }
          />
        </Box>

        <Grid container spacing={2}>
          <Grid size={{ xs: 6, sm: 4 }}>
            <Typography variant="caption" color="text.secondary">
              Placa
            </Typography>

            <Typography fontWeight={600}>
              {veiculo.placa}
            </Typography>
          </Grid>

          <Grid size={{ xs: 6, sm: 4 }}>
            <Typography variant="caption" color="text.secondary">
              Ano
            </Typography>

            <Typography fontWeight={600}>
              {veiculo.ano}
            </Typography>
          </Grid>

          <Grid size={{ xs: 6, sm: 4 }}>
            <Typography variant="caption" color="text.secondary">
              Cor
            </Typography>

            <Typography fontWeight={600}>
              {veiculo.cor}
            </Typography>
          </Grid>

          <Grid size={{ xs: 6, sm: 4 }}>
            <Typography variant="caption" color="text.secondary">
              Tipo
            </Typography>

            <Typography fontWeight={600}>
              {veiculo.tipo}
            </Typography>
          </Grid>

          <Grid size={{ xs: 6, sm: 4 }}>
            <Typography variant="caption" color="text.secondary">
              Preço
            </Typography>

            <Typography fontWeight={600}>
              {formatarPreco(veiculo.preco)}
            </Typography>
          </Grid>

          <Grid size={{ xs: 6, sm: 4 }}>
            <Typography variant="caption" color="text.secondary">
              Quilometragem
            </Typography>

            <Typography fontWeight={600}>
              {veiculo.quilometragem} km
            </Typography>
          </Grid>
        </Grid>

        <Divider sx={{ my: 3 }} />

        <Box
          sx={{
            display: 'flex',
            justifyContent: 'space-between',
            alignItems: {
              xs: 'stretch',
              sm: 'center',
            },
            flexDirection: {
              xs: 'column',
              sm: 'row',
            },
            gap: 2,
            mb: 2,
          }}
        >
          <Typography variant="h6" fontWeight={700}>
            Histórico de proprietários
          </Typography>

          <Button variant="contained" 
          onClick={onAdicionarProprietario}>
            Adicionar proprietário
          </Button>
        </Box>

        <TableContainer
          component={Paper}
          variant="outlined"
          sx={{ overflowX: 'auto' }}
        >
          <Table sx={{ minWidth: 650 }}>
            <TableHead>
              <TableRow>
                <TableCell>Nome</TableCell>
                <TableCell>CPF</TableCell>
                <TableCell>Aquisição</TableCell>
                <TableCell>Venda</TableCell>
                <TableCell>Observação</TableCell>
              </TableRow>
            </TableHead>

            <TableBody>
              {veiculo.proprietarios?.map((proprietario) => (
                <TableRow key={proprietario.id}>
                  <TableCell>
                    {proprietario.nomeCompleto}
                  </TableCell>

                  <TableCell>
                    {proprietario.cpf}
                  </TableCell>

                  <TableCell>
                    {formatarData(proprietario.dataAquisicao)}
                  </TableCell>

                  <TableCell>
                    {formatarData(proprietario.dataVenda)}
                  </TableCell>

                  <TableCell>
                    {proprietario.observacao || '-'}
                  </TableCell>
                </TableRow>
              ))}

              {!veiculo.proprietarios?.length && (
                <TableRow>
                  <TableCell
                    colSpan={5}
                    align="center"
                    sx={{ py: 4 }}
                  >
                    Nenhum proprietário cadastrado.
                  </TableCell>
                </TableRow>
              )}
            </TableBody>
          </Table>
        </TableContainer>
      </DialogContent>

      <DialogActions>
        <Button onClick={onClose}>
          Fechar
        </Button>
      </DialogActions>
    </Dialog>
  );
}