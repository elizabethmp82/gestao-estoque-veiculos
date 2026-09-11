import { useEffect, useState } from 'react';
import {
  Box,
  Button,
  Chip,
  Container,
  MenuItem,
  Paper,
  Stack,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  TextField,
  Typography,
} from '@mui/material';

import AddIcon from '@mui/icons-material/Add';
import VisibilityIcon from '@mui/icons-material/Visibility';
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/Delete';
import VeiculoFormModal from '../components/VeiculoFormModal';
import VeiculoDetalheModal from '../components/VeiculoDetalheModal';
import ProprietarioFormModal from '../components/ProprietarioFormModal';

import api from '../services/api';

export default function VeiculosPage() {
  const [veiculos, setVeiculos] = useState([]);
  const [marca, setMarca] = useState('');
  const [situacao, setSituacao] = useState('');
  const [modalVeiculoAberto, setModalVeiculoAberto] = useState(false);
  const [modalDetalheAberto, setModalDetalheAberto] = useState(false);
  const [veiculoSelecionado, setVeiculoSelecionado] = useState(null);
  const [modalProprietarioAberto, setModalProprietarioAberto] = useState(false);

  useEffect(() => {
    async function buscarVeiculos() {
      try {
        const response = await api.get('/Veiculos');

        setVeiculos(response.data);
      } catch (error) {
        console.error('Erro ao carregar veículos:', error);
      }
    }

    buscarVeiculos();
  }, []);

  async function filtrarVeiculos() {
    try {
      const response = await api.get('/Veiculos', {
        params: {
          marca: marca || undefined,
          situacao: situacao || undefined,
        },
      });

      setVeiculos(response.data);
    } catch (error) {
      console.error('Erro ao filtrar veículos:', error);
    }
  }

  function formatarPreco(valor) {
    return Number(valor).toLocaleString('pt-BR', {
      style: 'currency',
      currency: 'BRL',
    });
  }

 async function visualizarVeiculo(id) {
  try {
    const response = await api.get(`/Veiculos/${id}`);

    setVeiculoSelecionado(response.data);
    setModalDetalheAberto(true);
  } catch (error) {
    console.error('Erro ao buscar veículo:', error);
  }
    
}

  async function atualizarVeiculoSelecionado() {
  if (!veiculoSelecionado) {
    return;
  }

  try {
    const response = await api.get(
      `/Veiculos/${veiculoSelecionado.id}`,
    );

    setVeiculoSelecionado(response.data);
  } catch (error) {
    console.error(
      'Erro ao atualizar dados do veículo:',
      error,
    );
  }
}

  return (
    <Box
      sx={{
        minHeight: '100vh',
        bgcolor: 'background.default',
        py: {
          xs: 2,
          md: 4,
        },
      }}
    >
      <Container maxWidth="xl">
        <Stack
          direction={{
            xs: 'column',
            md: 'row',
          }}
          justifyContent="space-between"
          alignItems={{
            xs: 'stretch',
            md: 'center',
          }}
          spacing={2}
          mb={3}
        >
          <Box>
            <Typography
              variant="h4"
              fontWeight={700}
              sx={{
                fontSize: {
                  xs: '1.6rem',
                  md: '2rem',
                },
              }}
            >
              Gestão de Estoque de Veículos
            </Typography>

            <Typography
              variant="body2"
              color="text.secondary"
            >
              Gerencie veículos e proprietários.
            </Typography>
          </Box>

          <Button
             variant="contained"
             startIcon={<AddIcon />}
             size="large"
               onClick={() => setModalVeiculoAberto(true)}
              sx={{
                 alignSelf: {
                 xs: 'stretch',
                 md: 'auto',
              },
            }}
           >
               Novo veículo
         </Button>
        </Stack>

        <Paper
          sx={{
            p: {
              xs: 2,
              md: 3,
            },
            mb: 3,
          }}
        >
          <Stack
            direction={{
              xs: 'column',
              md: 'row',
            }}
            spacing={2}
          >
            <TextField
              label="Marca"
              value={marca}
              onChange={(event) => setMarca(event.target.value)}
              fullWidth
            />

            <TextField
              select
              label="Situação"
              value={situacao}
              onChange={(event) => setSituacao(event.target.value)}
              fullWidth
            >
              <MenuItem value="">Todas</MenuItem>
              <MenuItem value="Disponível">Disponível</MenuItem>
              <MenuItem value="Reservado">Reservado</MenuItem>
              <MenuItem value="Vendido">Vendido</MenuItem>
            </TextField>

            <Button
              variant="outlined"
              onClick={filtrarVeiculos}
              sx={{
                minWidth: {
                  xs: '100%',
                  md: 140,
                },
              }}
            >
              Filtrar
            </Button>
          </Stack>
        </Paper>

        <Paper>
          <TableContainer
            sx={{
              overflowX: 'auto',
            }}
          >
            <Table
              sx={{
                minWidth: 900,
              }}
            >
              <TableHead>
                <TableRow>
                  <TableCell>Marca</TableCell>
                  <TableCell>Modelo</TableCell>
                  <TableCell>Ano</TableCell>
                  <TableCell>Placa</TableCell>
                  <TableCell>Tipo</TableCell>
                  <TableCell>Preço</TableCell>
                  <TableCell>Quilometragem</TableCell>
                  <TableCell>Situação</TableCell>
                  <TableCell align="center">
                    Ações
                  </TableCell>
                </TableRow>
              </TableHead>

              <TableBody>
                {veiculos.map((veiculo) => (
                  <TableRow key={veiculo.id}>
                    <TableCell>
                      {veiculo.marca}
                    </TableCell>

                    <TableCell>
                      {veiculo.modelo}
                    </TableCell>

                    <TableCell>
                      {veiculo.ano}
                    </TableCell>

                    <TableCell>
                      {veiculo.placa}
                    </TableCell>

                    <TableCell>
                      {veiculo.tipo}
                    </TableCell>

                    <TableCell>
                      {formatarPreco(veiculo.preco)}
                    </TableCell>

                    <TableCell>
                      {veiculo.quilometragem} km
                    </TableCell>

                    <TableCell>
                      <Chip
                        label={veiculo.situacao}
                        color={
                          veiculo.situacao === 'Disponível'
                            ? 'success'
                            : veiculo.situacao === 'Vendido'
                              ? 'default'
                              : 'warning'
                        }
                        size="small"
                      />
                    </TableCell>

                    <TableCell align="center">
                      <Stack
                        direction="row"
                        spacing={1}
                        justifyContent="center"
                      >
                        <Button
                          size="small"
                          startIcon={<VisibilityIcon />}
                          onClick={() => visualizarVeiculo(veiculo.id)}
                        >
                          Ver
                        </Button>

                        <Button
                          size="small"
                          startIcon={<EditIcon />}
                        >
                          Editar
                        </Button>

                        <Button
                          size="small"
                          color="error"
                          startIcon={<DeleteIcon />}
                        >
                          Excluir
                        </Button>
                      </Stack>
                    </TableCell>
                  </TableRow>
                ))}

                {veiculos.length === 0 && (
                  <TableRow>
                    <TableCell
                      colSpan={9}
                      align="center"
                      sx={{
                        py: 5,
                        color: 'text.secondary',
                      }}
                    >
                      Nenhum veículo encontrado.
                    </TableCell>
                  </TableRow>
                )}
              </TableBody>
            </Table>
          </TableContainer>
        </Paper>
      </Container>
      <VeiculoFormModal
        open={modalVeiculoAberto}
        onClose={() => setModalVeiculoAberto(false)}
        onSalvo={filtrarVeiculos}
      />
      <VeiculoDetalheModal
        open={modalDetalheAberto}
        onClose={() => setModalDetalheAberto(false)}
        veiculo={veiculoSelecionado}
        onAdicionarProprietario={() => setModalProprietarioAberto(true)}
       />
     <ProprietarioFormModal
          open={modalProprietarioAberto}
          onClose={() => setModalProprietarioAberto(false)}
          veiculoId={veiculoSelecionado?.id}
          onSalvo={atualizarVeiculoSelecionado}
       />
    </Box>
  );
}