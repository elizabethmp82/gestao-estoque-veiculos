import {
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  Divider,
  Grid,
  MenuItem,
  TextField,
  Typography,
} from '@mui/material';

import { useEffect, useState } from 'react';
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
  situacao: 'Disponível',

  novoProprietario: {
    nomeCompleto: '',
    cpf: '',
    dataAquisicao: '',
    observacao: '',
  },
};

export default function VeiculoFormModal({
  open,
  onClose,
  onSalvo,
  veiculo,
}) {
  const [form, setForm] = useState(estadoInicial);
  const [salvando, setSalvando] = useState(false);
  

  const editando = Boolean(veiculo);

  const novaVenda =
  editando &&
  veiculo?.situacao !== 'Vendido' &&
  form.situacao === 'Vendido';

  useEffect(() => {
    if (veiculo) {
      setForm({
        marca: veiculo.marca ?? '',
        modelo: veiculo.modelo ?? '',
        ano: veiculo.ano ?? '',
        cor: veiculo.cor ?? '',
        preco: veiculo.preco ?? '',
        tipo: veiculo.tipo ?? '',
        placa: veiculo.placa ?? '',
        quilometragem: veiculo.quilometragem ?? '',
        situacao: veiculo.situacao ?? 'Disponível',

        novoProprietario: {
          nomeCompleto: '',
          cpf: '',
          dataAquisicao: '',
          observacao: '',
        },
      });
    } else {
      setForm(estadoInicial);
    }
  }, [veiculo, open]);

  function alterarCampo(event) {
    const { name, value } = event.target;

    setForm((anterior) => ({
      ...anterior,
      [name]: value,
    }));
  }

  function alterarProprietario(event) {
    const { name, value } = event.target;

    setForm((anterior) => ({
      ...anterior,
      novoProprietario: {
        ...anterior.novoProprietario,
        [name]: value,
      },
    }));
  }

  async function salvar() {
    try {
      setSalvando(true);

      if (editando) {
        const payload = {
          marca: form.marca,
          modelo: form.modelo,
          ano: Number(form.ano),
          cor: form.cor,
          preco: Number(form.preco),
          tipo: form.tipo,
          situacao: form.situacao,
          quilometragem: Number(form.quilometragem),
          novoProprietario:
          novaVenda
              ? form.novoProprietario
              : null,
        };

        await api.put(`/Veiculos/${veiculo.id}`, payload);
      } else {
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
      }

      setForm(estadoInicial);

      await onSalvo();
      onClose();
    } catch (error) {
      alert(
        error.response?.data?.mensagem ||
          'Erro ao salvar veículo.'
      );
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
        {editando ? 'Editar veículo' : 'Cadastrar veículo'}
      </DialogTitle>

      <DialogContent dividers>
        <Grid container spacing={2} sx={{ mt: 0.5 }}>
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
              disabled={editando}
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

          {editando && (
            <Grid size={{ xs: 12, md: 4 }}>
              <TextField
                select
                label="Situação"
                name="situacao"
                value={form.situacao}
                onChange={alterarCampo}
                fullWidth
                required
              >
                <MenuItem value="Disponível">
                  Disponível
                </MenuItem>

                <MenuItem value="Reservado">
                  Reservado
                </MenuItem>

                <MenuItem value="Vendido">
                  Vendido
                </MenuItem>
              </TextField>
            </Grid>
          )}
        </Grid>

        {novaVenda && (
          <>
            <Divider sx={{ my: 3 }} />

            <Typography
              variant="h6"
              fontWeight={700}
              sx={{ mb: 2 }}
            >
              Novo proprietário
            </Typography>

            <Grid container spacing={2}>
              <Grid size={{ xs: 12, md: 6 }}>
                <TextField
                  label="Nome completo"
                  name="nomeCompleto"
                  value={form.novoProprietario.nomeCompleto}
                  onChange={alterarProprietario}
                  fullWidth
                  required
                />
              </Grid>

              <Grid size={{ xs: 12, md: 6 }}>
                <TextField
                  label="CPF"
                  name="cpf"
                  value={form.novoProprietario.cpf}
                  onChange={alterarProprietario}
                  fullWidth
                  required
                />
              </Grid>

              <Grid size={{ xs: 12, md: 6 }}>
                <TextField
                  label="Data de aquisição"
                  name="dataAquisicao"
                  type="date"
                  value={
                    form.novoProprietario.dataAquisicao
                  }
                  onChange={alterarProprietario}
                  fullWidth
                  required
                  slotProps={{
                    inputLabel: {
                      shrink: true,
                    },
                  }}
                />
              </Grid>

              <Grid size={{ xs: 12, md: 6 }}>
                <TextField
                  label="Observação"
                  name="observacao"
                  value={
                    form.novoProprietario.observacao
                  }
                  onChange={alterarProprietario}
                  fullWidth
                />
              </Grid>
            </Grid>
          </>
        )}
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