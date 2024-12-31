import { useState } from 'react';

import Autoplay from 'embla-carousel-autoplay';
import { useNavigate } from 'react-router-dom';

import { useUserMovies } from 'Api/queries/useUserMovies';
import { Button, ButtonVariant } from 'Components/Button';
import { Carousel, CarouselContent, CarouselItem } from 'Components/Carousel';
import { Dialog, DialogContent, DialogHeader, DialogTitle } from 'Components/Dialog';
import { Skeleton } from 'Components/Skeleton';
import { Movie } from 'Types/movie';
import { getYouTubeId } from 'Utils/getYoutubeId';

export const MovieCarousel = () => {
  const [selectedMovie, setSelectedMovie] = useState<Nullable<Movie>>(null);

  const { data: movies, isFetching } = useUserMovies();

  const navigate = useNavigate();

  const handleMovieClick = (movie: Movie) => {
    setSelectedMovie(movie);
  };

  const closeDialog = () => {
    setSelectedMovie(null);
  };

  if (isFetching)
    return (
      <div className="h-[35rem] py-8">
        <Skeleton rows={6} />
      </div>
    );

  return (
    <>
      <Carousel
        opts={{
          align: 'center',
          loop: true,
        }}
        plugins={[
          Autoplay({
            delay: 5000,
          }),
        ]}
        className="mx-auto max-w-[1100px]"
      >
        <CarouselContent>
          {(movies || []).slice(0, 5).map(movie => (
            <CarouselItem key={movie.id} className="min-h-[500px] w-full basis-full">
              <div
                className="relative h-full w-full cursor-pointer"
                onClick={() => handleMovieClick(movie)}
              >
                <div
                  className="h-full w-full bg-cover bg-top"
                  style={{
                    backgroundImage: `url(${movie.backgroundUrl})`,
                    filter: 'brightness(0.7)',
                  }}
                />
              </div>
            </CarouselItem>
          ))}
        </CarouselContent>
      </Carousel>

      <Dialog open={!!selectedMovie} onOpenChange={closeDialog}>
        {selectedMovie && (
          <DialogContent className="sm:max-w-[800px]">
            <DialogHeader className="flex">
              <DialogTitle>{selectedMovie.title}</DialogTitle>
            </DialogHeader>
            <div className="aspect-video">
              <iframe
                width="100%"
                height="100%"
                src={`https://www.youtube.com/embed/${getYouTubeId(selectedMovie.trailerUrl)}`}
                allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture"
                allowFullScreen
              />
            </div>
            <Button
              variant={ButtonVariant.Secondary}
              onClick={() => navigate(`/movie/${selectedMovie.id}`)}
            >
              Zobacz więcej
            </Button>
          </DialogContent>
        )}
      </Dialog>
    </>
  );
};
