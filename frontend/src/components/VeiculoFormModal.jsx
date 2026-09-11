import {
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  Grid,
  MenuItem,
  TextField,
} from '@mui/material';
import { useState } from 'react';
import api from '../services/api';

const estadoInicial = {
  marca: '',
  modelo: '',
  ano: '',
  cor: '',
  preco: '',
  tipo: '',
  placa: '',
  quilometragem: '',
};

export default function VeiculoFormModal({
  open,
  onClose,
  onSalvo,
}) {
  const [form, setForm] = useState(estadoInicial);
  const [salvando, setSalvando] = useState(false);

  function alterarCampo(event) {
    const { name, value } = event.target;

    setForm((anterior) => ({
      ...anterior,
      [name]: value,
    }));
  }

  async function salvar() {
    try {
      setSalvando(true);

      await api.post('/Veiculos', {
        marca: form.marca,
        modelo: form.modelo,
        ano: Number(form.ano),
        cor: form.cor,
        preco: Number(form.preco),
        tipo: form.tipo,
        placa: form.placa,
        quilometragem: Number(form.quilometragem),
      });

      setForm(estadoInicial);

      onSalvo();
      onClose();
    } catch (error) {
      console.error('Erro ao cadastrar veículo:', error);
    } finally {
      setSalvando(false);
    }
  }

  function fechar() {
    setForm(estadoInicial);
    onClose();
  }

  return (
    <Dialog
      open={open}
      onClose={fechar}
      fullWidth
      maxWidth="md"
    >
      <DialogTitle>
        Cadastrar veículo
      </DialogTitle>

      <DialogContent dividers>
        <Grid
          container
          spacing={2}
          sx={{ mt: 0.5 }}
        >
          <Grid size={{ xs: 12, md: 6 }}>
            <TextField
              label="Marca"
              name="marca"
              value={form.marca}
              onChange={alterarCampo}
              fullWidth
              required
            />
          </Grid>

          <Grid size={{ xs: 12, md: 6 }}>
            <TextField
              label="Modelo"
              name="modelo"
              value={form.modelo}
              onChange={alterarCampo}
              fullWidth
              required
            />
          </Grid>

          <Grid size={{ xs: 12, md: 4 }}>
            <TextField
              label="Ano"
              name="ano"
              type="number"
              value={form.ano}
              onChange={alterarCampo}
              fullWidth
              required
            />
          </Grid>

          <Grid size={{ xs: 12, md: 4 }}>
            <TextField
              label="Cor"
              name="cor"
              value={form.cor}
              onChange={alterarCampo}
              fullWidth
              required
            />
          </Grid>

          <Grid size={{ xs: 12, md: 4 }}>
            <TextField
              select
              label="Tipo"
              name="tipo"
              value={form.tipo}
              onChange={alterarCampo}
              fullWidth
              required
            >
              <MenuItem value="Hatch">Hatch</MenuItem>
              <MenuItem value="Sedan">Sedan</MenuItem>
              <MenuItem value="SUV">SUV</MenuItem>
              <MenuItem value="Picape">Picape</MenuItem>
            </TextField>
          </Grid>

          <Grid size={{ xs: 12, md: 4 }}>
            <TextField
              label="Placa"
              name="placa"
              value={form.placa}
              onChange={alterarCampo}
              fullWidth
              required
            />
          </Grid>

          <Grid size={{ xs: 12, md: 4 }}>
            <TextField
              label="Preço"
              name="preco"
              type="number"
              value={form.preco}
              onChange={alterarCampo}
              fullWidth
              required
            />
          </Grid>

          <Grid size={{ xs: 12, md: 4 }}>
            <TextField
              label="Quilometragem"
              name="quilometragem"
              type="number"
              value={form.quilometragem}
              onChange={alterarCampo}
              fullWidth
              required
            />
          </Grid>
        </Grid>
      </DialogContent>

      <DialogActions>
        <Button
          onClick={fechar}
          color="inherit"
        >
          Cancelar
        </Button>

        <Button
          onClick={salvar}
          variant="contained"
          disabled={salvando}
        >
          {salvando ? 'Salvando...' : 'Salvar'}
        </Button>
      </DialogActions>
    </Dialog>
  );
}