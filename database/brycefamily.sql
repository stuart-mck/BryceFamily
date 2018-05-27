-- Database: brycefamily

-- DROP DATABASE brycefamily;

CREATE DATABASE brycefamily
    WITH 
    OWNER = postgres
    ENCODING = 'UTF8'
    LC_COLLATE = 'en_US.utf8'
    LC_CTYPE = 'en_US.utf8'
    TABLESPACE = pg_default
    CONNECTION LIMIT = -1;


-- User: brycefamily
-- DROP USER brycefamily;

CREATE USER brycefamily WITH
  LOGIN
  NOSUPERUSER
  INHERIT
  NOCREATEDB
  NOCREATEROLE
  NOREPLICATION;	

-- Table: public.event

-- DROP TABLE public.event;

CREATE TABLE public.event
(
    title character varying(256) COLLATE pg_catalog."default" NOT NULL,
    start_date date NOT NULL,
    end_date date NOT NULL,
    details text COLLATE pg_catalog."default" NOT NULL,
    organiser_name character varying(256) COLLATE pg_catalog."default" NOT NULL,
    organiser_contact character varying(256) COLLATE pg_catalog."default" NOT NULL,
    event_type smallint NOT NULL,
    date_created timestamp with time zone NOT NULL,
    last_updated timestamp with time zone NOT NULL,
    id uuid NOT NULL,
    CONSTRAINT event_pkey PRIMARY KEY (id)
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public.event
    OWNER to postgres;

GRANT INSERT, SELECT, UPDATE, DELETE ON TABLE public.event TO brycefamily;

GRANT ALL ON TABLE public.event TO postgres;

-- Table: public.family_union

-- DROP TABLE public.family_union;

CREATE TABLE public.family_union
(
    id uuid NOT NULL,
    partner_1 integer,
    partner_2 integer,
    date_of_marriage date,
    year_of_marriage integer,
    date_of_divorce date,
    year_of_divorce integer,
    date_created timestamp with time zone NOT NULL,
    last_updated timestamp with time zone NOT NULL,
    CONSTRAINT union_pkey PRIMARY KEY (id)
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public.family_union
    OWNER to postgres;

GRANT INSERT, SELECT, UPDATE, DELETE ON TABLE public.family_union TO brycefamily;

GRANT ALL ON TABLE public.family_union TO postgres;	

-- Table: public.gallery

-- DROP TABLE public.gallery;

CREATE TABLE public.gallery
(
    title character varying(256) COLLATE pg_catalog."default" NOT NULL,
    gallery_date date,
    fk_person_id integer,
    summary text COLLATE pg_catalog."default",
    date_created timestamp with time zone NOT NULL,
    last_updated timestamp with time zone NOT NULL,
    id uuid NOT NULL,
    CONSTRAINT gallery_pkey PRIMARY KEY (id),
    CONSTRAINT fk_person FOREIGN KEY (fk_person_id)
        REFERENCES public.person (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
        NOT VALID
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public.gallery
    OWNER to postgres;

GRANT INSERT, SELECT, UPDATE, DELETE ON TABLE public.gallery TO brycefamily;

GRANT ALL ON TABLE public.gallery TO postgres;

-- Index: fki_fk_person

-- DROP INDEX public.fki_fk_person;

CREATE INDEX fki_fk_person
    ON public.gallery USING btree
    (fk_person_id)
    TABLESPACE pg_default;
	
-- Table: public.person

-- DROP TABLE public.person;

CREATE TABLE public.person
(
    date_of_birth date,
    is_spouse boolean NOT NULL,
    first_name character varying(256) COLLATE pg_catalog."default" NOT NULL,
    id integer NOT NULL,
    date_of_death integer,
    state character varying(256) COLLATE pg_catalog."default",
    post_code character(4) COLLATE pg_catalog."default",
    year_of_death integer,
    email_address character varying(256) COLLATE pg_catalog."default",
    last_name character varying(256) COLLATE pg_catalog."default" NOT NULL,
    middle_name character varying(256) COLLATE pg_catalog."default",
    suburb character varying(256) COLLATE pg_catalog."default",
    year_of_birth integer,
    date_created timestamp with time zone NOT NULL,
    last_updated timestamp with time zone NOT NULL,
    gender smallint,
    maiden_name character varying(256) COLLATE pg_catalog."default",
    phone character varying(32) COLLATE pg_catalog."default",
    address_1 character varying(256) COLLATE pg_catalog."default",
    address_2 character varying(256) COLLATE pg_catalog."default",
    occupation character varying(256) COLLATE pg_catalog."default",
    CONSTRAINT person_pkey PRIMARY KEY (id)
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public.person
    OWNER to postgres;

GRANT INSERT, SELECT, UPDATE, DELETE ON TABLE public.person TO brycefamily;

GRANT ALL ON TABLE public.person TO postgres;

-- Index: fki_parents

-- DROP INDEX public.fki_parents;

CREATE INDEX fki_parents
    ON public.person USING btree
    (parents_id)
    TABLESPACE pg_default;

-- Table: public.resource_person

-- DROP TABLE public.resource_person;

CREATE TABLE public.resource_person
(
    person_id integer NOT NULL,
    resource_id uuid NOT NULL,
    CONSTRAINT resource_person_pkey PRIMARY KEY (person_id, resource_id),
    CONSTRAINT fk_person FOREIGN KEY (person_id)
        REFERENCES public.person (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
        NOT VALID,
    CONSTRAINT fk_person_resource FOREIGN KEY (resource_id)
        REFERENCES public.resource_reference (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE CASCADE,
    CONSTRAINT fk_resource_person FOREIGN KEY (person_id)
        REFERENCES public.person (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
        NOT VALID
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public.resource_person
    OWNER to postgres;

GRANT INSERT, SELECT, UPDATE, DELETE ON TABLE public.resource_person TO brycefamily;

GRANT ALL ON TABLE public.resource_person TO postgres;

-- Index: fki_fk_person_resource

-- DROP INDEX public.fki_fk_person_resource;

CREATE INDEX fki_fk_person_resource
    ON public.resource_person USING btree
    (resource_id)
    TABLESPACE pg_default;

-- Index: fki_fk_resource_person

-- DROP INDEX public.fki_fk_resource_person;

CREATE INDEX fki_fk_resource_person
    ON public.resource_person USING btree
    (person_id)
    TABLESPACE pg_default;


-- Table: public.resource_reference

-- DROP TABLE public.resource_reference;

CREATE TABLE public.resource_reference
(
    id uuid NOT NULL,
    location character varying(256) COLLATE pg_catalog."default" NOT NULL,
    mime_type character varying(32) COLLATE pg_catalog."default" NOT NULL,
    title character varying(256) COLLATE pg_catalog."default",
    file_name character varying(256) COLLATE pg_catalog."default" NOT NULL,
    date_created timestamp with time zone NOT NULL,
    last_updated timestamp with time zone NOT NULL,
    gallery_id uuid,
    CONSTRAINT resource_reference_pkey PRIMARY KEY (id),
    CONSTRAINT resource_reference_gallery FOREIGN KEY (gallery_id)
        REFERENCES public.gallery (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public.resource_reference
    OWNER to postgres;

GRANT INSERT, SELECT, UPDATE, DELETE ON TABLE public.resource_reference TO brycefamily;

GRANT ALL ON TABLE public.resource_reference TO postgres;

-- Index: fki_resource_reference_gallery

-- DROP INDEX public.fki_resource_reference_gallery;

CREATE INDEX fki_resource_reference_gallery
    ON public.resource_reference USING btree
    (gallery_id)
    TABLESPACE pg_default;

-- Table: public.story

-- DROP TABLE public.story;

CREATE TABLE public.story
(
    id uuid NOT NULL,
    author character varying(256) COLLATE pg_catalog."default" NOT NULL,
    title character varying(256) COLLATE pg_catalog."default" NOT NULL,
    contents text COLLATE pg_catalog."default" NOT NULL,
    version integer NOT NULL,
    published boolean NOT NULL,
    date_created timestamp with time zone NOT NULL,
    last_updated timestamp with time zone NOT NULL,
    CONSTRAINT story_pkey PRIMARY KEY (id)
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public.story
    OWNER to postgres;

GRANT INSERT, SELECT, UPDATE, DELETE ON TABLE public.story TO brycefamily;

GRANT ALL ON TABLE public.story TO postgres;

-- Table: public.story_person

-- DROP TABLE public.story_person;

CREATE TABLE public.story_person
(
    person_id integer NOT NULL,
    story_id uuid NOT NULL,
    CONSTRAINT story_person_pkey PRIMARY KEY (person_id, story_id),
    CONSTRAINT person_story FOREIGN KEY (story_id)
        REFERENCES public.story (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
        NOT VALID,
    CONSTRAINT story_person FOREIGN KEY (person_id)
        REFERENCES public.person (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public.story_person
    OWNER to postgres;

GRANT INSERT, SELECT, UPDATE, DELETE ON TABLE public.story_person TO brycefamily;

GRANT ALL ON TABLE public.story_person TO postgres;

-- Index: fki_person_story

-- DROP INDEX public.fki_person_story;

CREATE INDEX fki_person_story
    ON public.story_person USING btree
    (story_id)
    TABLESPACE pg_default;	
	

-- Table: public.union_offspring

-- DROP TABLE public.union_offspring;

CREATE TABLE public.union_offspring
(
    family_union_id uuid NOT NULL,
    person_id integer,
    CONSTRAINT union_offspring_pkey PRIMARY KEY (family_union_id),
    CONSTRAINT union_child FOREIGN KEY (person_id)
        REFERENCES public.person (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION,
    CONSTRAINT union_relationship FOREIGN KEY (family_union_id)
        REFERENCES public.family_union (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public.union_offspring
    OWNER to postgres;

-- Index: fki_union_child

-- DROP INDEX public.fki_union_child;

CREATE INDEX fki_union_child
    ON public.union_offspring USING btree
    (person_id)
    TABLESPACE pg_default;

GRANT INSERT, SELECT, UPDATE, DELETE ON TABLE public.union_offspring TO brycefamily;

GRANT ALL ON TABLE public.union_offspring TO postgres;
	