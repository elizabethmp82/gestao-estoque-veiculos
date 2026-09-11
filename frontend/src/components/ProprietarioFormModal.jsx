import { useState } from 'react';
import {
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  TextField,
  Stack,
} from '@mui/material';

import api from '../services/api';

const estadoInicial = {
  nomeCompleto: '',
  cpf: '',
  dataAquisicao: '',
  observacao: '',
};

export default function ProprietarioFormModal({
  open,
  onClose,
  veiculoId,
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

      await api.post('/Proprietarios', {
        veiculoId,
        nomeCompleto: form.nomeCompleto,
        cpf: form.cpf,
        dataAquisicao: form.dataAquisicao,
        observacao: form.observacao,
      });

      setForm(estadoInicial);

      onSalvo();
      onClose();
    } catch (error) {
      console.error('Erro ao salvar proprietário:', error);
    } finally {
      setSalvando(false);
    }
  }

  return (
    <Dialog
      open={open}
      onClose={onClose}
      fullWidth
      maxWidth="sm"
    >
      <DialogTitle>
        Cadastrar proprietário
      </DialogTitle>

      <DialogContent dividers>
        <Stack spacing={2} sx={{ mt: 1 }}>
          <TextField
            label="Nome completo"
            name="nomeCompleto"
            value={form.nomeCompleto}
            onChange={alterarCampo}
            fullWidth
            required
          />

          <TextField
            label="CPF"
            name="cpf"
            value={form.cpf}
            onChange={alterarCampo}
            fullWidth
            required
          />

          <TextField
            label="Data de aquisição"
            name="dataAquisicao"
            type="date"
            value={form.dataAquisicao}
            onChange={alterarCampo}
            fullWidth
            required
            slotProps={{
              inputLabel: {
                shrink: true,
              },
            }}
          />

          <TextField
            label="Observação"
            name="observacao"
            value={form.observacao}
            onChange={alterarCampo}
            multiline
            rows={3}
            fullWidth
          />
        </Stack>
      </DialogContent>

      <DialogActions>
        <Button
          onClick={onClose}
          color="inherit"
        >
          Cancelar
        </Button>

        <Button
          variant="contained"
          onClick={salvar}
          disabled={salvando}
        >
          {salvando ? 'Salvando...' : 'Salvar'}
        </Button>
      </DialogActions>
    </Dialog>
  );
}